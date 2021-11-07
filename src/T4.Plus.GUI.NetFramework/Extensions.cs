using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TextTemplateTransformationFramework.T4.Plus.GUI
{
    public static class Extensions
    {
        public static void SetText(this TextBox instance, string text)
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() => instance.Text = text));
            }
            else
            {
                instance.Text = text;
            }
        }

        public static void Append(this TextBox instance, string text)
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() => instance.AppendText(text)));
            }
            else
            {
                instance.AppendText(text);
            }
        }

        public static void SetEnabled(this Control instance, bool enabled)
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() => instance.Enabled = enabled));
            }
            else
            {
                instance.Enabled = enabled;
            }
        }

        public static void Add(this ListView instance, string text, IList<string> lines)
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() =>
                    {
                        var lvi = instance.Items.Add(text);
                        lvi.Tag = lines;
                    }));
            }
            else
            {
                var lvi = instance.Items.Add(text);
                lvi.Tag = lines;
            }
        }

        public static void AddWithSubItems(this ListView instance, string text, IEnumerable<string> subItems)
        {
            if (subItems == null)
            {
                throw new ArgumentNullException(nameof(subItems));
            }

            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() =>
                {
                    var lvi = instance.Items.Add(text);
                    foreach (var subItem in subItems)
                    {
                        lvi.SubItems.Add(subItem);
                    }
                }));
            }
            else
            {
                var lvi = instance.Items.Add(text);
                foreach (var subItem in subItems)
                {
                    lvi.SubItems.Add(subItem);
                }
            }
        }

        public static void AddColumn(this ListView instance, string text)
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() => instance.Columns.Add(text)));
            }
            else
            {
                instance.Columns.Add(text);
            }
        }

        public static void ClearItems(this ListView instance)
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() => instance.Items.Clear()));
            }
            else
            {
                instance.Items.Clear();
            }
        }

        public static void ClearColumns(this ListView instance)
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() => instance.Columns.Clear()));
            }
            else
            {
                instance.Columns.Clear();
            }
        }

        public static void SetSelectedObject(this PropertyGrid instance, object selectedObject)
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() => instance.SelectedObject = selectedObject));
            }
            else
            {
                instance.SelectedObject = selectedObject;
            }
        }

        public static void DoRefresh(this PropertyGrid instance)
        {
            if (instance.InvokeRequired)
            {
                instance.Invoke(new Action(() => instance.Refresh()));
            }
            else
            {
                instance.Refresh();
            }
        }

        public static void AutoResize(this ListView listView)
        {
            //Prevents flickering
            if (listView.InvokeRequired)
            {
                listView.Invoke(new Action(() => listView.BeginUpdate()));
            }
            else
            {
                listView.BeginUpdate();
            }

            var columnSizes = new Dictionary<int, int>();

            //Auto size using header
            if (listView.InvokeRequired)
            {
                listView.Invoke(new Action(() => listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)));
            }
            else
            {
                listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }

            //Grab column size based on header
            foreach (ColumnHeader colHeader in listView.Columns)
            {
                if (listView.InvokeRequired)
                {
                    listView.Invoke(new Action(() => columnSizes.Add(colHeader.Index, colHeader.Width)));
                }
                else
                {
                    columnSizes.Add(colHeader.Index, colHeader.Width);
                }
            }

            //Auto size using data
            if (listView.InvokeRequired)
            {
                listView.Invoke(new Action(() => listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)));
            }
            else
            {
                listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            }

            if (listView.InvokeRequired)
            {
                listView.Invoke(new Action(() => SetColumnWidths(listView, columnSizes)));
            }
            else
            {
                SetColumnWidths(listView, columnSizes);
            }

            if (listView.InvokeRequired)
            {
                listView.Invoke(new Action(() => listView.EndUpdate()));
            }
            else
            {
                listView.EndUpdate();
            }
        }

        private static void SetColumnWidths(ListView listView, Dictionary<int, int> columnSizes)
        {
            //Grab comumn size based on data and set max width
            foreach (ColumnHeader colHeader in listView.Columns)
            {
                if (columnSizes.TryGetValue(colHeader.Index, out int nColWidth))
                {
                    colHeader.Width = Math.Max(nColWidth, colHeader.Width);
                }
                else
                {
                    //Default to 50
                    colHeader.Width = Math.Max(50, colHeader.Width);
                }
            }
        }
    }
}
