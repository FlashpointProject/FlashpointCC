using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlashpointCurator
{
    public partial class ColumnPriorityForm : Form
    {
        private DataGridView dataGridView;

        public ColumnPriorityForm(DataGridView dataGridView)
        {
            this.dataGridView = dataGridView;
            InitializeComponent();
            checkedListBox.ItemCheck += CheckedListBox_ItemCheck;
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        }

        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string name = checkedListBox.Items[e.Index].ToString();
            dataGridView.Columns[name].Visible = e.NewValue == CheckState.Checked;
        }

        private void ColumnPriorityForm_Load(object sender, EventArgs e)
        {
            foreach (DataGridViewColumn col in dataGridView.Columns)
            {
                checkedListBox.Items.Add(col.Name, col.Visible);
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            MoveItem(-1);
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            MoveItem(1);
        }

        public void MoveItem(int direction)
        {
            // Checking selected item
            if (checkedListBox.SelectedItem == null || checkedListBox.SelectedIndex < 0)
                return; // No selected item - nothing to do

            // Calculate new index using move direction
            int index = checkedListBox.SelectedIndex;
            int newIndex = index + direction;

            // Checking bounds of the range
            if (newIndex < 0 || newIndex >= checkedListBox.Items.Count)
                return; // Index out of range - nothing to do

            object selected = checkedListBox.SelectedItem;
            var checkState = checkedListBox.GetItemCheckState(index);

            // Removing removable element
            checkedListBox.Items.Remove(selected);
            // Insert it in new position
            checkedListBox.Items.Insert(newIndex, selected);
            // Restore item check state
            checkedListBox.SetItemCheckState(newIndex, checkState);
            // Restore selection
            checkedListBox.SetSelected(newIndex, true);

            // Apply to DataGridView
            dataGridView.Columns[selected.ToString()].DisplayIndex = newIndex;
        }
    }
}
