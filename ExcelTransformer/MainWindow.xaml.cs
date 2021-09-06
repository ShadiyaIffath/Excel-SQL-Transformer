using ExcelTransformer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

                TransformationDetails transformer = new TransformationDetails(excel_file.Text);
                int rows = transformer.readHeaderRow();

                if(rows != 0 && transformer.columns.Count != 0)
                {
                    QueryGenerator queryGenerator = new QueryGenerator(table_name.Text, transformer.columns);

                    for (int i = 0; i < rows; i++)
                    {
                        transformer.readExcelSheet(i);
                        if (input_query.IsChecked == true)
                            sql_text.Text += queryGenerator.generateInsertQuery(transformer.data);
                        else
                            sql_text.Text += queryGenerator.generateUpdateQuery(transformer.data);
                    }
                }

            }
            catch (UnauthorizedAccessException)
            {
                error.Content = ErrorMessages.NO_PERMISSION;
                return;
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
