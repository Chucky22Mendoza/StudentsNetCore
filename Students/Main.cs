using Logic;
using Logic.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Students {
    public partial class Main : Form {

        private LStudent student;

        public Main() {
            InitializeComponent();

            var listTextBox = new List<TextBox>();
            listTextBox.Add(txtID);
            listTextBox.Add(txtName);
            listTextBox.Add(txtLastname);
            listTextBox.Add(txtEmail);

            var listLabel = new List<Label>();
            listLabel.Add(lblID);
            listLabel.Add(lblName);
            listLabel.Add(lblLastname);
            listLabel.Add(lblEmail);
            listLabel.Add(lblPagination);

            Object[] objects = {
                imgBox,
                Properties.Resources.logo,
                tableStudents,
                numControl
            };

            student = new LStudent(listTextBox, listLabel, objects);
        }

        private void imgBox_Click(object sender, EventArgs e) {
            student.uploadImage.renderImage(imgBox);
        }

        private void txtID_TextChanged(object sender, EventArgs e) {
            if (txtID.Text.Equals("")) {
                lblID.ForeColor = Color.LightSlateGray;
            } else {
                lblID.ForeColor = Color.Green;
                lblID.Text = "ID";
            }
        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e) {
            student.textBoxEvent.numberKeyPress(e);
        }

        private void txtName_TextChanged(object sender, EventArgs e) {
            if (txtName.Text.Equals("")) {
                lblName.ForeColor = Color.LightSlateGray;
            } else {
                lblName.ForeColor = Color.Green;
                lblName.Text = "Name";
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e) {
            student.textBoxEvent.textKeyPress(e);
        }

        private void txtLastname_TextChanged(object sender, EventArgs e) {
            if (txtLastname.Text.Equals("")) {
                lblLastname.ForeColor = Color.LightSlateGray;
            } else {
                lblLastname.ForeColor = Color.Green;
                lblLastname.Text = "Lastname";
            }
        }

        private void txtLastname_KeyPress(object sender, KeyPressEventArgs e) {
            student.textBoxEvent.textKeyPress(e);
        }

        private void txtEmail_TextChanged(object sender, EventArgs e) {
            if (txtEmail.Text.Equals("")) {
                lblEmail.ForeColor = Color.LightSlateGray;
            } else {
                lblEmail.ForeColor = Color.Green;
                lblEmail.Text = "Email";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e) {
            student.Register();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) {
            student.SearchStudents(txtSearch.Text);
        }

        private void btnFirst_Click(object sender, EventArgs e) {
            student.Paginator("First");
        }

        private void btnBefore_Click(object sender, EventArgs e) {
            student.Paginator("Before");
        }

        private void btnNext_Click(object sender, EventArgs e) {
            student.Paginator("Next");
        }

        private void btnLast_Click(object sender, EventArgs e) {
            student.Paginator("Last");
        }

        private void numControl_ValueChanged(object sender, EventArgs e) {
            student.SetPages();
        }

        private void tableStudents_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (tableStudents.Rows.Count != 0) {
                student.GetStudent();
            }
        }

        private void tableStudents_KeyUp(object sender, KeyEventArgs e) {
            if (tableStudents.Rows.Count != 0) {
                student.GetStudent();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e) {
            student.DeleteRecord();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            student.ClearFields();
        }
    }
}
