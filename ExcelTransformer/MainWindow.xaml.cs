using ExcelTransformer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelTransformer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            excel_file.Text = "";
            table_name.Text = "";
            error.Content = "";
            sql_progress.Visibility = Visibility.Hidden;
            sql_text.Text = "";
        }

        private void confirm_btn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(excel_file.Text))
            {
                error.Content = ErrorMessages.FILE_PATH_REQUIRED;
                return;
            }

            if (String.IsNullOrWhiteSpace(table_name.Text))
            {
                error.Content = ErrorMessages.TABLE_NAME_MISSING;
                return;
            }

            try
            {
                if (!File.Exists(excel_file.Text))
                {
                    error.Content = ErrorMessages.PATH_NOT_FOUND;
                    return;
                }

                new Thread(() =>
                {
                    int rows = 0;
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        error.Content = "";
                        TransformationDetails._filepath = excel_file.Text;
                    }));
                    rows = TransformationDetails.getRowCount() - 1;
                    if (rows != 0)
                    {

                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            sql_progress.Maximum = rows - 1;
                            sql_progress.Value = 0;
                            sql_progress.Visibility = Visibility.Visible;
                        }));
                        QueryGenerator.columns = TransformationDetails.readExcelSheet(1);

                        Parallel.For(2, rows, (i, pls) =>
                        {
                            var data = TransformationDetails.readExcelSheet(i);
                            if (data.Count == 0)
                            {
                                throw new ValueUnavailableException();
                            }
                            this.Dispatcher.Invoke((Action)(() =>
                            {
                                if (input_query.IsChecked == true)
                                    sql_text.Text += QueryGenerator.generateInsertQuery(table_name.Text, data);

                                sql_progress.Value += 1;
                            }));
                        });
                    }
                    else
                    {
                        error.Content = ErrorMessages.RECORDS_NOT_FOUND;
                    }
                }).Start();
            }
            catch (UnauthorizedAccessException)
            {
                error.Content = ErrorMessages.NO_PERMISSION;
            }
            catch (ValueUnavailableException)
            {
                error.Content = ErrorMessages.RECORDS_NOT_FOUND;
            }
        }

        private void attach_file_button_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();

            openFileDlg.Filter = "Excel Worksheets 2003 (*.xls)|*.xls|,Excel Worksheets 2007 (*.xlsx)|*.xlsx|, CSV Files (*.csv)|*.csv";

            Nullable<bool> result = openFileDlg.ShowDialog();

            if (result == true)
            {
                excel_file.Text = openFileDlg.FileName;
            }
        }

    }
}
