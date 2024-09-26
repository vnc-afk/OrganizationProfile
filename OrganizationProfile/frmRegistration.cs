using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrganizationProfile
{
    public partial class frmRegistration : Form
    {
        public frmRegistration()
        {
            InitializeComponent();
        }

        private String _FullName;  
        private int _Age;         
        private long _ContactNo;   
        private long _StudentNo;

        private void btnRegistar_Click(object sender, EventArgs e)
        {
            try
            {

                long studentNo = StudentNumber(txtStudentNo.Text);
                if (studentNo == -1) return;

                string selectedProgram = cbPrograms.Text;
                if (string.IsNullOrWhiteSpace(selectedProgram))
                {
                    MessageBox.Show("Please select a program.");
                    return;
                }

                _FullName = FullName(txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text);
                if (string.IsNullOrEmpty(_FullName)) return;

                int age = Age(txtAge.Text);
                if (age == -1) return;

                if (datePickerBirthday.Value > DateTime.Today.AddYears(-15))
                {
                    MessageBox.Show("Birthday error ");
                    return;
                }

                string selectedGender = cbGender.Text;
                if (string.IsNullOrWhiteSpace(selectedGender))
                {
                    MessageBox.Show("Please select a gender.");
                    return;
                }
                long contactNo = ContactNo(txtContactNo.Text);
                if (contactNo == -1) return; 

               
                StudentInformationClass.SetFullName = $"{txtLastName.Text}, {txtFirstName.Text}, {txtMiddleInitial.Text}";
                StudentInformationClass.SetStudentNo = StudentNumber(txtStudentNo.Text); 
                StudentInformationClass.SetProgram = cbPrograms.Text;
                StudentInformationClass.SetGender = cbGender.Text;
                StudentInformationClass.SetContactNo = ContactNo(txtContactNo.Text);
                StudentInformationClass.SetAge = Age(txtAge.Text); 
                StudentInformationClass.SetBirthday = datePickerBirthday.Value.ToString("yyyy-MM-dd");

                frmConfirmation frm = new frmConfirmation();
                frm.ShowDialog();
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Registration process completed.");
            }
        }

        private void frmRegistration_Load(object sender, EventArgs e)
        {

            string[] ListOfProgram = new string[]
             {
                "BS Information Technology",
                "BS Computer Science",
                "BS Information Systems",
                "BS in Accountancy",
                "BS in Hospitality Management",
                "BS in Tourism Management"
             };

            try
            {
                cbPrograms.Items.AddRange(ListOfProgram);
            }
            catch (IndexOutOfRangeException ex)
            {
                MessageBox.Show("An error occurred while loading the programs: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Program loading process completed.");
            }
        }


        public long StudentNumber(string studNum)
        {
            try
            {
                if (long.TryParse(studNum, out _StudentNo))
                {
                    return _StudentNo;
                }
                else
                {
                    throw new FormatException("Invalid student number format.");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return -1;  
            }
            catch (OverflowException ex)
            {
                MessageBox.Show("Error: Student number is too large. " + ex.Message);
                return -1; 
            }
            finally
            {
                Console.WriteLine("Student number processing complete.");
            }
        }

        public long ContactNo(string contact)
        {
            try
            {
                if (Regex.IsMatch(contact, @"^\+?\d{9,13}$"))
                {
                    string processedContact = contact.StartsWith("+") ? contact.Substring(1) : contact;
                    _ContactNo = long.Parse(processedContact);
                    return _ContactNo;
                }
                else
                {
                    throw new FormatException("Invalid contact number format. Please enter a valid number starting with +63 and followed by 9 digits.");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return -1; 
            }
            catch (OverflowException ex)
            {
                MessageBox.Show("Error: Contact number is too large. " + ex.Message);
                return -1; 
            }
            finally
            {
                Console.WriteLine("Contact number processing complete.");
            }
        }

        public string FullName(string lastName, string firstName, string middleInitial)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(firstName))
                {
                    throw new ArgumentNullException("First name and last name cannot be empty.");
                }

                if (!Regex.IsMatch(lastName, @"^[a-zA-Z]+$") || !Regex.IsMatch(firstName, @"^[a-zA-Z]+$"))
                {
                    throw new FormatException("First name and last name can only contain letters.");
                }

                if (!string.IsNullOrWhiteSpace(middleInitial) && !Regex.IsMatch(middleInitial, @"^[a-zA-Z]$"))
                {
                    throw new FormatException("Middle initial can only contain one letter.");
                }

                return $"{lastName}, {firstName}" +
                       (string.IsNullOrWhiteSpace(middleInitial) ? "" : $", {middleInitial}");
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return string.Empty;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return string.Empty;
            }
            finally
            {
                Console.WriteLine("Full name processing complete.");
            }
        }

        public int Age(string age)
        {
            try
            {
                if (int.TryParse(age, out _Age))
                {
                    return _Age;
                }
                else
                {
                    throw new FormatException("Invalid age format.");
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return -1;  
            }
            catch (OverflowException ex)
            {
                MessageBox.Show("Error: Age is too large. " + ex.Message);
                return -1; 
            }
            finally
            {
                Console.WriteLine("Age processing complete.");
            }
        }
    }
}
