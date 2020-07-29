using Data;
using LinqToDB;
using LinqToDB.SqlQuery;
using Logic.Library;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ubiety.Dns.Core.Records;

namespace Logic {
    public class LStudent : Libraries {
        private List<TextBox> listTextBox;
        private List<Label> listLabel;
        private PictureBox image;
        private Bitmap _imageBitmap;
        private DataGridView _dataGridView;
        private NumericUpDown _numericUpDown;
        private Paginator<Student> _paginator;
        private string _action = "Insert";

        public LStudent(List<TextBox> listTextBox, List<Label> listLabel, object[] objects) {
            this.listTextBox = listTextBox;
            this.listLabel = listLabel;
            image = (PictureBox)objects[0];
            _imageBitmap = (Bitmap)objects[1];
            _dataGridView = (DataGridView)objects[2];
            _numericUpDown = (NumericUpDown)objects[3];
            ClearFields();
        }

        public void Register() {
            if (listTextBox[0].Text.Equals("")) {
                listLabel[0].Text = "NID is required";
                listLabel[0].ForeColor = Color.Red;
                listTextBox[0].Focus();
            } else {
                if (listTextBox[1].Text.Equals("")) {
                    listLabel[1].Text = "Name is required";
                    listLabel[1].ForeColor = Color.Red;
                    listTextBox[1].Focus();
                } else {
                    if (listTextBox[2].Text.Equals("")) {
                        listLabel[2].Text = "Lastname is required";
                        listLabel[2].ForeColor = Color.Red;
                        listTextBox[2].Focus();
                    } else {
                        if (listTextBox[3].Text.Equals("")) {
                            listLabel[3].Text = "Email is required";
                            listLabel[3].ForeColor = Color.Red;
                            listTextBox[3].Focus();
                        } else {
                            if (!textBoxEvent.validateEmailFormat(listTextBox[3].Text)) {
                                listLabel[3].Text = "Email not valid";
                                listLabel[3].ForeColor = Color.Red;
                                listTextBox[3].Focus();
                            } else {
                                var user = _Student.Where(obj => obj.email.Equals(listTextBox[3].Text)).ToList();
                                if (user.Count.Equals(0)) {
                                    SaveRecord();
                                } else {
                                    if (user[0].ID.Equals(_idStudent)) {
                                        SaveRecord();
                                    } else {
                                        listLabel[3].Text = "Email already recorded";
                                        listLabel[3].ForeColor = Color.Red;
                                        listTextBox[3].Focus();
                                    }
                                    
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SaveRecord() {
            BeginTransactionAsync();
            try {
                var imageArray = uploadImage.ImageToByte(image.Image);
                switch (_action) {
                    case "Insert":
                        _Student.Value(obj => obj.nid, listTextBox[0].Text)
                            .Value(obj => obj.name, listTextBox[1].Text)
                            .Value(obj => obj.lastname, listTextBox[2].Text)
                            .Value(obj => obj.email, listTextBox[3].Text)
                            .Value(obj => obj.image, imageArray)
                            .Insert();
                        MessageBox.Show("Student added successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case "Update":
                        _Student.Where(obj => obj.ID.Equals(_idStudent))
                            .Set(obj => obj.nid, listTextBox[0].Text)
                            .Set(obj => obj.name, listTextBox[1].Text)
                            .Set(obj => obj.lastname, listTextBox[2].Text)
                            .Set(obj => obj.email, listTextBox[3].Text)
                            .Set(obj => obj.image, imageArray)
                            .Update();
                        MessageBox.Show("Student updated successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                }
                CommitTransaction();
                ClearFields();
            } catch (SqlException) {
                RollbackTransaction();
            }
        }

        private int _record_per_page = 2, _num_page = 1;

        public void SearchStudents(string field) {
            List<Student> query = new List<Student>();
            int start = (_num_page - 1) * _record_per_page;
            if (field.Equals("")) {
                query = _Student.ToList();
            } else {
                query = _Student.Where(obj => obj.nid.StartsWith(field) 
                                            || obj.name.StartsWith(field) 
                                            || obj.lastname.StartsWith(field)).ToList();
            }

            if (query.Count > 0) {
                _dataGridView.DataSource = query.Select(obj => new {
                    obj.ID,
                    obj.nid,
                    obj.name,
                    obj.lastname,
                    obj.email,
                    obj.image
                }).Skip(start).Take(_record_per_page).ToList();

                _dataGridView.Columns[0].Visible = false;
                _dataGridView.Columns[5].Visible = false;
                _dataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                _dataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            } else {
                _dataGridView.DataSource = query.Select(obj => new {
                    obj.ID,
                    obj.nid,
                    obj.name,
                    obj.lastname,
                    obj.email
                }).ToList();

                _dataGridView.Columns[0].Visible = false;
                _dataGridView.Columns[5].Visible = false;
                _dataGridView.Columns[1].DefaultCellStyle.BackColor = Color.WhiteSmoke;
                _dataGridView.Columns[3].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            }
        }

        private int _idStudent = 0;
        public void GetStudent() {
            _action = "Update";
            _idStudent = Convert.ToInt16(_dataGridView.CurrentRow.Cells[0].Value);
            listTextBox[0].Text = Convert.ToString(_dataGridView.CurrentRow.Cells[1].Value);
            listTextBox[1].Text = Convert.ToString(_dataGridView.CurrentRow.Cells[2].Value);
            listTextBox[2].Text = Convert.ToString(_dataGridView.CurrentRow.Cells[3].Value);
            listTextBox[3].Text = Convert.ToString(_dataGridView.CurrentRow.Cells[4].Value);
            try {
                byte[] arrayImage = (byte[])_dataGridView.CurrentRow.Cells[5].Value;
                image.Image = uploadImage.byteArrayToImage(arrayImage);
            } catch (Exception) {
                image.Image = _imageBitmap;
            }
        }

        private List<Student> listStudents;

        public void Paginator(string btnValue) {
            switch (btnValue) {
                case "First":
                    _num_page = _paginator.first();
                    break;
                case "Before":
                    _num_page = _paginator.before();
                    break;
                case "Next":
                    _num_page = _paginator.next();
                    break;
                case "Last":
                    _num_page = _paginator.last();
                    break;
            }
            SearchStudents("");
        }

        public void SetPages() {
            _num_page = 1;
            _record_per_page = (int)_numericUpDown.Value;
            var list = _Student.ToList();
            if (list.Count > 0) {
                _paginator = new Paginator<Student>(listStudents, listLabel[4], _record_per_page);
                SearchStudents("");
            }
        }

        public void DeleteRecord() {
            if (_idStudent.Equals(0)) {
                MessageBox.Show("Select a student", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                if (MessageBox.Show("Are you sure of this action?", "Delete student", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                    _Student.Where(obj => obj.ID.Equals(_idStudent)).Delete();
                    ClearFields();
                }
            }
        }

        public void ClearFields() {
            _action = "Insert";
            _num_page = 1;
            _idStudent = 0;
            image.Image = _imageBitmap;
            listLabel[0].Text = "NID";
            listLabel[1].Text = "Name";
            listLabel[2].Text = "Lastname";
            listLabel[3].Text = "Email";
            listLabel[0].ForeColor = Color.FromArgb(85, 85, 85);
            listLabel[1].ForeColor = Color.FromArgb(85, 85, 85);
            listLabel[2].ForeColor = Color.FromArgb(85, 85, 85);
            listLabel[3].ForeColor = Color.FromArgb(85, 85, 85);
            listTextBox[0].Text = "";
            listTextBox[1].Text = "";
            listTextBox[2].Text = "";
            listTextBox[3].Text = "";
            listStudents = _Student.ToList();
            if (listStudents.Count > 0) {
                _paginator = new Paginator<Student>(listStudents, listLabel[4], _record_per_page);
            }
            SearchStudents("");
        }
    }
}
