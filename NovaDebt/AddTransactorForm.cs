﻿using NovaDebt.Models.Enums;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

using static NovaDebt.DataSettings;

namespace NovaDebt
{
    public partial class AddTransactorForm : Form
    {
        public AddTransactorForm()
        {
            InitializeComponent();
        }

        private void AddTransactorForm_Load(object sender, EventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            this.btnAddConfirm.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);
            this.btnAddCancel.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);

            this.addSinceDatePicker.Format = DateTimePickerFormat.Custom;
            this.addSinceDatePicker.CustomFormat = "dd/MM/yyyy";
            this.addSinceDatePicker.Value = DateTime.UtcNow;

            this.addDueDatePicker.Format = DateTimePickerFormat.Custom;
            this.addDueDatePicker.CustomFormat = "dd/MM/yyyy";
            this.addDueDatePicker.Value = DateTime.UtcNow;

            // Setting the default select button as Debtor
            this.btnAddDebtor.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);
            this.btnAddDebtor.BackColor = Color.FromArgb(0, 208, 255);

            // Attaching an event will will warn the user upon cancel/exit.
            this.FormClosing += new FormClosingEventHandler(AlertUserOnExit);
        }

        private void btnAddConfirm_Click(object sender, EventArgs e)
        {
            bool areFieldsValid = ValidateInputFields();

            if (areFieldsValid)
            {
                string[] inputFields = new string[] {
                    addNameTextBox.Text,
                    addSinceDatePicker.Text,
                    addDueDatePicker.Text,
                    addPhoneTextBox.Text,
                    addEmailTextBox.Text,
                    addFacebookTextBox.Text,
                    addAmountTextBox.Text
                };

                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);

                for (int i = 0; i < inputFields.Length; i++)
                {
                    // Removing all unnecessary whitespaces.
                    inputFields[i] = inputFields[i].TrimStart().TrimEnd();
                    inputFields[i] = regex.Replace(inputFields[i], " ");
                }

                string name = inputFields[0];
                string since = inputFields[1];
                string dueDate = inputFields[2];
                string phone = inputFields[3];
                string email = inputFields[4];
                string facebook = inputFields[5];
                decimal amount = decimal.Parse(inputFields[6]);
                string transactorType = string.Empty;
                string path = TransactorsFilePath;

                if (btnAddDebtor.BackColor == Color.FromArgb(0, 208, 255))
                {
                    transactorType = TransactorType.Debtor.ToString();
                    XmlProcess.AddTransactorToXml(path, name, since, dueDate, phone, email, amount, facebook, transactorType);
                }
                else if (btnAddCreditor.BackColor == Color.FromArgb(0, 208, 255))
                {
                    transactorType = TransactorType.Creditor.ToString();
                    XmlProcess.AddTransactorToXml(path, name, since, dueDate, phone, email, amount, facebook, transactorType);
                }

                this.FormClosing -= AlertUserOnExit;
                this.Close();
            }
        }

        private void btnAddCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(MessageBoxText.ExitConfirmation,
                MessageBoxCaption.Exit,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.FormClosing -= AlertUserOnExit;
                this.Close();
            }
        }

        private void btnAddDebtor_Click(object sender, EventArgs e)
        {
            // other
            this.btnAddCreditor.FlatAppearance.BorderColor = Color.WhiteSmoke;
            this.btnAddCreditor.BackColor = Color.FromArgb(50, 50, 50);

            // this
            this.btnAddDebtor.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);
            this.btnAddDebtor.BackColor = Color.FromArgb(0, 208, 255);
        }

        private void btnAddCreditor_Click(object sender, EventArgs e)
        {
            // other
            this.btnAddDebtor.FlatAppearance.BorderColor = Color.WhiteSmoke;
            this.btnAddDebtor.BackColor = Color.FromArgb(50, 50, 50);

            // this
            this.btnAddCreditor.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);
            this.btnAddCreditor.BackColor = Color.FromArgb(0, 208, 255);
        }

        private bool ValidateInputFields()
        {
            //
            // Име - Name (Required)
            //
            Regex mainRegex = new Regex("^[a-zA-Z0-9., ]*$");
            Regex cyrillicRegex = new Regex("^[АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя0-9., ]*$");

            if (!mainRegex.IsMatch(this.addNameTextBox.Text)
             && !cyrillicRegex.IsMatch(this.addNameTextBox.Text))
            {
                MessageBox.Show(ErrorMessage.InvalidName,
                    MessageBoxCaption.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
            else if (string.IsNullOrEmpty(this.addNameTextBox.Text)
                  || string.IsNullOrWhiteSpace(this.addNameTextBox.Text))
            {
                MessageBox.Show(ErrorMessage.MissingName,
                    MessageBoxCaption.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Since
            //
            if (addSinceDatePicker.Value > addDueDatePicker.Value)
            {
                MessageBox.Show(ErrorMessage.InvalidSinceDate,
                    MessageBoxCaption.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Due Date
            //
            // One of the date validations is unnecessary.
            if (addDueDatePicker.Value < addSinceDatePicker.Value)
            {
                MessageBox.Show(ErrorMessage.InvalidDueDate,
                    MessageBoxCaption.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Тел № - PhoneNumber
            //
            mainRegex = new Regex("^[+0-9-() ]*$");

            if (!mainRegex.IsMatch(this.addPhoneTextBox.Text))
            {
                MessageBox.Show(ErrorMessage.InvalidPhoneNumber,
                    MessageBoxCaption.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Имейл - Email
            //
            mainRegex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (!mainRegex.IsMatch(this.addEmailTextBox.Text.Trim())
                && !string.IsNullOrEmpty(this.addEmailTextBox.Text.Trim())
                && !cyrillicRegex.IsMatch(this.addEmailTextBox.Text.Trim()))
            {
                MessageBox.Show(ErrorMessage.InvalidEmail,
                    MessageBoxCaption.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Фейсбук - Facebook
            //
            mainRegex = new Regex("^[A-z ]*$");

            if (!mainRegex.IsMatch(this.addFacebookTextBox.Text)
                && !cyrillicRegex.IsMatch(this.addFacebookTextBox.Text))
            {
                MessageBox.Show(ErrorMessage.InvalidFacebook,
                    MessageBoxCaption.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Количество - Amount (Required)
            //
            mainRegex = new Regex("^[0-9]+([.,][0-9]{1,2})?$");
            decimal amount;

            if (mainRegex.IsMatch(this.addAmountTextBox.Text.Trim()))
            {
                amount = decimal.Parse(addAmountTextBox.Text);

                if (amount < 0.01m || amount > 4294967295m)
                {
                    MessageBox.Show(string.Format(string.Format(ErrorMessage.InvalidAmountInterval, MinAmountValue, MaxAmountValue)),
                           MessageBoxCaption.Error,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);

                    return false;
                }
            }
            else if (string.IsNullOrEmpty(this.addAmountTextBox.Text) || string.IsNullOrWhiteSpace(this.addAmountTextBox.Text))
            {
                MessageBox.Show(ErrorMessage.MissingAmount,
                    MessageBoxCaption.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
            else
            {
                MessageBox.Show(ErrorMessage.InvalidAmount,
                    MessageBoxCaption.Error,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void AlertUserOnExit(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show(MessageBoxText.ExitConfirmation,
                   MessageBoxCaption.Exit,
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.FormClosing -= AlertUserOnExit;
                this.Close();
            }
            else if (dialog == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}