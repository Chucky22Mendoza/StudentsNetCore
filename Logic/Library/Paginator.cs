using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic.Library {
    public class Paginator<T> {
        private List<T> _dataList;
        private Label _label;
        private static int maxRecord, _record_per_page, pageCount, numPage = 1;

        public Paginator(List<T> dataList, Label label, int record_per_page) {
            _dataList = dataList;
            _label = label;
            _record_per_page = record_per_page;
            renderData();
        }

        private void renderData() {
            numPage = 1;
            maxRecord = _dataList.Count;
            pageCount = (maxRecord / _record_per_page);

            if ((maxRecord % _record_per_page) > 0) {
                pageCount += 1;
            }
            _label.Text = $"Pages 1/{pageCount}";
        }

        public int first() {
            numPage = 1;
            _label.Text = $"Pages {numPage}/{pageCount}";
            return numPage;
        }

        public int before() {
            if (numPage > 1) {
                numPage -= 1;
                _label.Text = $"Pages {numPage}/{pageCount}";
            }
            return numPage;
        }

        public int next() {
            if (numPage == pageCount) {
                numPage -= 1;
            }
            if (numPage < pageCount) {
                numPage += 1;
                _label.Text = $"Pages {numPage}/{pageCount}";
            }
            return numPage;
        }

        public int last() {
            numPage = pageCount;
            _label.Text = $"Pages {numPage}/{pageCount}";
            return numPage;
        }
    }
}
