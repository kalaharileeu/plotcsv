﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace PlotDVT
{
    public class DataCol
    {

        [XmlElement("Column")]
        public List<Column> namealiaslist;

        public DataCol()
        {
            namealiaslist = new List<Column>();
        }
    }
}
