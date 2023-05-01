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

namespace RSA_LuongTrungHieu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RSACryptoServiceProvider rsa;
        private string publicKeyXml;
        private string privateKeyXml;


        

        public MainWindow()
        {
            InitializeComponent();
            btnreset.Click += btnreset_Click;
        }

        //tạo khóa tự động
        private void btntaokhoa_Click(object sender, RoutedEventArgs e)
        {
            //tạo đối tượng RSACryptoServiceProvider rsa
            rsa = new RSACryptoServiceProvider();
            publicKeyXml = rsa.ToXmlString(false);
            privateKeyXml = rsa.ToXmlString(true);

            //Hiển thị thông tin khóa tương ứng
            txtpublickey.Text = publicKeyXml;
            txtprivatekey.Text = privateKeyXml;

        }

        //mã hóa 
        private void btnmahoa_Click(object sender, RoutedEventArgs e)
        {
            //khóa không rỗng
            if (rsa == null)
            {
                MessageBox.Show("Hãy tạo khóa trước!", "Lỗi");
                return;
            }

            // Lấy dữ liệu cần mã hóa và thực hiện mã hóa
            string inputData = txtdulieu.Text;
            if (!string.IsNullOrEmpty(inputData))
            {
            byte[] encryptedData = EncryptData(inputData, txtpublickey.Text);

            // Hiển thị kết quả mã hóa trên TextBox
            txtmahoa.Text = Convert.ToBase64String(encryptedData);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập dữ liệu!", "Lỗi");
            }
        }

        private byte[] EncryptData(string inputData, string text)
        {
            // Tạo đối tượng RSACryptoServiceProvider từ khóa công khai
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyXml);

            // Chuyển đổi dữ liệu cần mã hóa sang kiểu byte[]
            byte[] dataToEncrypt = Encoding.UTF8.GetBytes(inputData);

            // Mã hóa dữ liệu và trả về kết quả
            return rsa.Encrypt(dataToEncrypt, false);

        }

        //giải mã 
        private void btngiaima_Click(object sender, RoutedEventArgs e)
        {
            if (rsa == null)
            {
                MessageBox.Show("Hãy tạo khóa trước!", "Lỗi");
                return;
            }

            // Lấy dữ liệu cần giải mã và thực hiện giải mã
            string inputData = txtmahoa.Text;
            if (!string.IsNullOrEmpty(inputData))
            {
            byte[] encryptedData = Convert.FromBase64String(inputData);
            string decryptedData = DecryptData(encryptedData, txtprivatekey.Text);

            // Hiển thị kết quả giải mã trên TextBox
            txtgiaima.Text = decryptedData;
            }
            else
            {
                MessageBox.Show("Vui lòng nhập dữ liệu!", "Lỗi");
            }
        }

        private string DecryptData(byte[] encryptedData, string text)
        {
            // Tạo đối tượng RSACryptoServiceProvider từ khóa bí mật
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(privateKeyXml);

            // Giải mã dữ liệu và chuyển đổi sang kiểu chuỗi
            byte[] decryptedData = rsa.Decrypt(encryptedData, false);
            return Encoding.UTF8.GetString(decryptedData);
        }

        private void btnthoat_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Bạn chắc chắn muốn thoát chương trình?", "Xác nhận thoát chương trình", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }

        }

        private void btnreset_Click(object sender, RoutedEventArgs e)
        {
            txtprivatekey.Text = string.Empty;
            txtpublickey.Text = string.Empty;
            txtdulieu.Text = string.Empty;
            txtmahoa.Text = string.Empty;
            txtgiaima.Text = string.Empty;
        }
    }
}
