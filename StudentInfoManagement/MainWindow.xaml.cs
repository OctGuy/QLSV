using StudentInfoManagement.Models;
using System.Collections.ObjectModel;
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

namespace StudentInfoManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // DBSet for DataGrid
        public ObservableCollection<StudentInfo>? Students { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadStudentData(); // Load Data from DB to DataGrid
        }

        private void LoadStudentData()
        {
            using (var context = new QlsvContext())
            {
                Students = new ObservableCollection<StudentInfo>(context.StudentInfos.ToList());
                studentInfoGrid.ItemsSource = Students;
            }
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            // Check if one of these boxes hasn't filled yet
            if (string.IsNullOrWhiteSpace(studentCodeTxtBox.Text)
                || string.IsNullOrWhiteSpace(studentNameTxtBox.Text)
                || string.IsNullOrWhiteSpace(phoneNumber.Text)
                || string.IsNullOrWhiteSpace(addressTxtBox.Text)
                || birthdayBox.SelectedDate == null
                || genderComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            string studentCode = studentCodeTxtBox.Text;
            string studentName = studentNameTxtBox.Text;
            string phone = phoneNumber.Text;
            string address = addressTxtBox.Text;

            DateTime dt = birthdayBox.SelectedDate ?? DateTime.Now;

            ComboBoxItem selectedGender = (ComboBoxItem)genderComboBox.SelectedItem;
            string gender = selectedGender.Content?.ToString() ?? "Giới tính thứ 3";

            using (var context = new QlsvContext())
            {
                // Check if a student code have existed in Database
                if (context.StudentInfos.Any(u => u.StudentCode == studentCode))
                {
                    MessageBox.Show("Mã số sinh viên này đã tồn tại.");
                }
                // add new student info
                else
                {
                    // Create new student object
                    var newStudent = new StudentInfo
                    {
                        StudentCode = studentCode,
                        StudentName = studentName,
                        Phone = phone,
                        Address = address,
                        Birthday = dt,
                        Gender = gender
                    };
                    context.StudentInfos.Add(newStudent); // Add new info into DB
                    context.SaveChanges(); // Save

                    LoadStudentData(); // Reload data to DataGrid
                }
            }
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            // Check if one of these boxes hasn't filled yet
            if (string.IsNullOrWhiteSpace(studentCodeTxtBox.Text)
                || string.IsNullOrWhiteSpace(studentNameTxtBox.Text)
                || string.IsNullOrWhiteSpace(phoneNumber.Text)
                || string.IsNullOrWhiteSpace(addressTxtBox.Text)
                || birthdayBox.SelectedDate == null
                || genderComboBox.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            string currentStudentCode = studentCodeTxtBox.Text;

            using (var context = new QlsvContext())
            {
                var existStudent = context.StudentInfos.FirstOrDefault(u => u.StudentCode == currentStudentCode);

                if (existStudent != null)
                {
                    string newStudentCode = studentCodeTxtBox.Text;

                    if (newStudentCode != currentStudentCode)
                    {
                        if (context.StudentInfos.Any(u => u.StudentCode == newStudentCode))
                        {
                            MessageBox.Show("Mã số sinh viên đã tồn tại");
                            return;
                        }
                    }

                    existStudent.StudentName = studentNameTxtBox.Text;
                    existStudent.Birthday = birthdayBox.SelectedDate;
                    ComboBoxItem selectedGender = (ComboBoxItem)genderComboBox.SelectedItem;
                    existStudent.Gender = selectedGender.Content?.ToString() ?? "Giới tính thứ 3";
                    existStudent.Phone = phoneNumber.Text;
                    existStudent.Address = addressTxtBox.Text;

                    context.SaveChanges();
                    LoadStudentData();
                    MessageBox.Show("Thông tin đã được cập nhật.");
                }
            }
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (studentInfoGrid.SelectedItem is StudentInfo selectedStudent)
            {
                var warningBox = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên có mã số là '{selectedStudent.StudentCode}'?", 
                                       "Xác nhận xóa", 
                                       MessageBoxButton.YesNo, 
                                       MessageBoxImage.Warning);

                if (warningBox == MessageBoxResult.Yes)
                {
                    using (var context = new QlsvContext())
                    {
                        var removedStudent = context.StudentInfos.FirstOrDefault
                            (u => u.StudentCode ==  selectedStudent.StudentCode);

                        if (removedStudent != null)
                        {
                            context.StudentInfos.Remove(removedStudent);
                            context.SaveChanges();

                            LoadStudentData();
                            MessageBox.Show("Xóa sinh viên thành công");
                        }
                    }
                }
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Exit 
        }

        private void studentChoose(object sender, SelectionChangedEventArgs e)
        {

            // When choosing any row in datagrid, all infos will be filled in these boxes 
            if (studentInfoGrid.SelectedItem is StudentInfo selectedStudent)
            {
                studentCodeTxtBox.Text = selectedStudent.StudentCode;
                studentNameTxtBox.Text = selectedStudent.StudentName;
                addressTxtBox.Text = selectedStudent.Address;
                phoneNumber.Text = selectedStudent.Phone;
                birthdayBox.SelectedDate = selectedStudent.Birthday;
                genderComboBox.SelectedItem = genderComboBox.Items
                    .Cast<ComboBoxItem>()
                    .FirstOrDefault(item => item.Content.ToString() == selectedStudent.Gender);
            }
        }
    }
}