using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ListViewTest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private bool stopRefreshControls = false;
        private bool dataChanged = false;

        public Window1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// addButton click event handler.
        /// Add a new row to the ListView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            setDataChanged(true);
            AddRow();
        }

        /// <summary>
        /// Adds a blank row to the ListView
        /// </summary>
        private void AddRow()
        {
            listView1.Items.Add(new ListViewData("", ""));
            listView1.SelectedIndex = listView1.Items.Count - 1;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox1.Focus();
        }

        /// <summary>
        /// removeButton click event handler
        /// Removes the selected row from the ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            setDataChanged(true);
            int selectedIndex = listView1.SelectedIndex;

            listView1.Items.Remove(listView1.SelectedItem);

            // if no rows left, add a blank row
            if (listView1.Items.Count == 0)
            {
                AddRow();
            }
            else if (selectedIndex <= listView1.Items.Count - 1) // otherwise select next row
            {
                listView1.SelectedIndex = selectedIndex;
            }
            else // not above cases? Select last row
            {
                listView1.SelectedIndex = listView1.Items.Count - 1;
            }
        }

        /// <summary>
        /// textBox1 TextChanged event handler
        /// Updates the ListView row with current Text 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListView(textBox1.Text, textBox2.Text);
        }

        /// <summary>
        /// Refreshses the ListView row with given values
        /// </summary>
        /// <param name="value1">Value for column 1</param>
        /// <param name="value2">Value for column 2</param>
        private void RefreshListView(string value1, string value2)
        {
            ListViewData lvc = (ListViewData)listView1.SelectedItem; //new ListViewClass(value1, value2);
            if (lvc != null && !stopRefreshControls)
            {
                setDataChanged(true);

                lvc.Col1 = value1;
                lvc.Col2 = value2;
                
                listView1.Items.Refresh();
            }
        }

        /// <summary>
        /// textBox2 TextChanged event handler
        /// Updates the ListView row with current Text 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshListView(textBox1.Text, textBox2.Text);
        }

        /// <summary>
        /// listView1 SelectionChnaged event handler.
        /// Updates the textboxes with values in the row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewData lvc = (ListViewData)listView1.SelectedItem;
            if (lvc != null)
            {
                stopRefreshControls = true;
                textBox1.Text = lvc.Col1;
                textBox2.Text = lvc.Col2;
                stopRefreshControls = false;
            }
        }

        /// <summary>
        /// Window Loaded event handler
        /// Loads data into ListView, and selecta a row.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ShowData();

            if (listView1.Items.Count == 0)
            {
                AddRow();
            }
            else
            {
                listView1.SelectedIndex = 0;
            }
            setDataChanged(false);
            textBox1.Focus();
        }

        /// <summary>
        /// Shows(Loads) data into the ListView
        /// </summary>
        private void ShowData()
        {
            MyData md = new MyData();
            listView1.Items.Clear();

            foreach (var row in md.GetRows())
            {
                listView1.Items.Add(row);
            }
        }

        /// <summary>
        /// saveButton click event handler.
        /// Saves data from ListView, if it is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            saveButton.IsEnabled = false;
            
            if (dataChanged)
            {
                MyData md = new MyData();
                md.Save(listView1.Items);
                setDataChanged(false);
            }

            // saveButton.IsEnabled = true;
        }

        /// <summary>
        /// closeButton click event handler.
        /// Closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// okButton click event handler.
        /// Saves the data, and closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            okButton.IsEnabled = false;
            if (dataChanged)
            {
                MyData md = new MyData();
                md.Save(listView1.Items);
                setDataChanged(false);
            }
            this.Close();
        }

        /// <summary>
        /// Sets the window into a DataChanged status.
        /// </summary>
        /// <param name="value"></param>
        private void setDataChanged(bool value)
        {
            dataChanged = value;
            saveButton.IsEnabled = value;
        }

        /// <summary>
        /// Window closing event handler.
        /// Prompts the user to save data, if it is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (dataChanged)
            {
                string message = "Your changes are not saved. Do you want to save it now?";
                MessageBoxResult result = MessageBox.Show(message,
                        this.Title,
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    MyData md = new MyData();
                    md.Save(listView1.Items);
                    setDataChanged(false);
                }
                else if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (result == MessageBoxResult.No)
                {
                    // do nothing
                }
            }
        }

        /// <summary>
        /// textBox1 KeyDown event handler.
        /// Restores old value, if Esc key is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                RestoreOldValue(sender);
            }
        }

        /// <summary>
        /// Restores old value, saved in Tag property, to textbox.
        /// </summary>
        /// <param name="sender"></param>
        private void RestoreOldValue(object sender)
        {
            TextBox myText = (TextBox)sender;

            if (myText.Text != myText.Tag.ToString())
            {
                myText.Text = myText.Tag.ToString();
                myText.SelectAll();
            }
        }

        /// <summary>
        /// Saves the current value to the Tag property of textbox.
        /// </summary>
        /// <param name="sender"></param>
        private void StoreCurrentValue(object sender)
        {
            TextBox myText = (TextBox)sender;
            myText.Tag = myText.Text;
        }

        /// <summary>
        /// textBox1 GotFocus event handler.
        /// Store the current value and select all text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_GotFocus(object sender, RoutedEventArgs e)
        {
            StoreCurrentValue(sender);
            textBox1.SelectAll();
        }

        /// <summary>
        /// textBox2 GotFocus event handler.
        /// Store the current value and select all text.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_GotFocus(object sender, RoutedEventArgs e)
        {
            StoreCurrentValue(sender);
            textBox2.SelectAll();
        }

        /// <summary>
        /// textBox2 KeyDown event handler.
        /// Restores old value, if Esc key is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                RestoreOldValue(sender);
            }
        }
    }
}
