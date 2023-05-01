using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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


namespace RSADemo_Nhom5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RSACryptoServiceProvider rsa;
        private string publicKeyXml;
        private string privateKeyXml;
        private object statusLabel;
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btntaokhoa_Click(object sender, RoutedEventArgs e)
        {
            //tạo đối tượng RSACryptoServiceProvider rsa
            rsa = new RSACryptoServiceProvider();
             publicKeyXml = rsa.ToXmlString(false);
             privateKeyXml = rsa.ToXmlString(true);

            //Hiển thị thông tin khóa tương ứng
            txtkpub.Text = publicKeyXml;
            txtkpr.Text = privateKeyXml;
        }

        private void btnmahoa_Click(object sender, RoutedEventArgs e)
        {
            string inputData = txtnhapdl.Text;
            //Khóa không rỗng
            if(rsa == null)
            {
                MessageBox.Show("Hãy tạo khóa trước!", "Lỗi");
                return;
            }    
            // Nếu dữ liệu cần mã hóa không rỗng
            if (!string.IsNullOrEmpty(inputData))
            {
                // Mã hóa dữ liệu và hiển thị kết quả trên giao diện
                byte[] encryptedData = EncryptData(inputData, publicKeyXml);
                txtmahoa.Text = Convert.ToBase64String(encryptedData);
                MessageBox.Show("Mã hóa thành công!","Thông báo");
            }
            else
            {
                MessageBox.Show("Vui lòng nhập dữ liệu!","Lỗi");
            }
        }

        private byte[] EncryptData(string inputData, string publicKeyXml)
        {
            // Tạo đối tượng RSACryptoServiceProvider từ khóa công khai
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyXml);

            // Chuyển đổi dữ liệu cần mã hóa sang kiểu byte[]
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(inputData);

            // Mã hóa dữ liệu và trả về kết quả
            return rsa.Encrypt(dataToEncrypt, false);
        }


            private string DecryptData(byte[] encryptedData, string privateKeyXml)
        {
            // Tạo đối tượng RSACryptoServiceProvider từ khóa bí mật
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKeyXml);

            // Giải mã dữ liệu và chuyển đổi sang kiểu chuỗi
            byte[] decryptedData = rsa.Decrypt(encryptedData, false);
            return Encoding.UTF8.GetString(decryptedData);
        }

        private void btngiaima_Click(object sender, RoutedEventArgs e)
        {
            String inputData = txtnhapdlgm.Text;


            // Nếu dữ liệu cần giải mã không rỗng
            if (!string.IsNullOrEmpty(inputData))
            {
                try
                {
                    // Giải mã dữ liệu và hiển thị kết quả trên giao diện
                    byte[] encryptedData = Convert.FromBase64String(inputData);
                    string decryptedData = DecryptData(encryptedData, privateKeyXml);
                    txtgiaima.Text = decryptedData;
                    MessageBox.Show("Giải mã thành công!","Thông báo");
                   
                }
                catch (FormatException)
                {
                    MessageBox.Show("Định dạng không hợp lệ!","Lỗi");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập dữ liệu!","Lỗi");
                
            }
        }

        private void btnthoat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn chắc chắn muốn thoát chương trình?", "Xác nhận thoát chương trình", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
        
    }


