using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Library {
    public class Libraries : Connection {
        public UploadImage uploadImage = new UploadImage();
        public TextBoxEvent textBoxEvent = new TextBoxEvent();
    }
}
