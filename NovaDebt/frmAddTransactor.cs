﻿using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NovaDebt
{
    public partial class frmAddTransactor : Form
    {
        private const decimal MinAmountValue = 0.01m;
        private const decimal MaxAmountValue = 4294967295m;
        private const string InvalidNameErrorMessage = "Името може да се състои само от букви и цифри.";
        private const string MissingNameErrorMessage = "Името e задължително.";
        private const string InvalidPhoneNumberErrorMessage = "Невалиден Тел №.";
        private const string InvalidEmailErrorMessage = "Невалиден Имейл.";
        private const string InvalidFacebookErrorMessage = "Невалиден Фейсбук.";
        private const string InvalidAmountInterval = "Количеството трябва да е в интервала {0} - {1}.";
        private const string MissingAmountErrorMessage = "Количеството е задължително.";
        private const string InvalidAmountErrorMessage = "Невалидно количество.";
        private const string ErrorMessageBoxCaption = "Грешка";
        private const string ExitMessageBoxCaption = "Изход";

        public frmAddTransactor()
        {
            InitializeComponent();
        }

        private void frmAddTransactor_Load(object sender, EventArgs e)
        {
            btnAddConfirm.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);
            btnAddCancel.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);

            // Setting the default selected button as Debtor
            btnAddDebtor.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);
            btnAddDebtor.BackColor = Color.FromArgb(0, 208, 255);

            this.FormClosing += new FormClosingEventHandler(WarnUserOnExit);
        }

        private void btnAddConfirm_Click(object sender, EventArgs e)
        {
            bool areFieldsValid = ValidateInputFields();

            if (areFieldsValid)
            {
                string[] inputFields = new string[] {
                    addNameTextBox.Text,
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

                // Spacing problem should be done by now.

                // It is time for the actual adding now.

                if (btnAddDebtor.BackColor == Color.FromArgb(0, 208, 255))
                {
                    // Add the transactor in the Debtors section (serialize it as XML and then refresh the data grid)
                }
                else if (btnAddCreditor.BackColor == Color.FromArgb(0, 208, 255))
                {
                    // The exact same but with a Creditor
                }
            }
        }

        private void btnAddCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Данните няма да бъдат запазени.{Environment.NewLine}Наистина ли искате да излезете?",
                ExitMessageBoxCaption,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Deattaching the FormClosing event handler method so it doesn't glitch.
                // This is done before every this.Close(); method to avoid potential bugs.
                // In this case only the button Cancel and the EXIT button.
                this.FormClosing -= WarnUserOnExit;
                this.Close();
            }
        }

        private void btnAddDebtor_Click(object sender, EventArgs e)
        {
            // other
            btnAddCreditor.FlatAppearance.BorderColor = Color.WhiteSmoke;
            btnAddCreditor.BackColor = Color.FromArgb(50, 50, 50);

            // this
            btnAddDebtor.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);
            btnAddDebtor.BackColor = Color.FromArgb(0, 208, 255);
        }

        private void btnAddCreditor_Click(object sender, EventArgs e)
        {
            // other
            btnAddDebtor.FlatAppearance.BorderColor = Color.WhiteSmoke;
            btnAddDebtor.BackColor = Color.FromArgb(50, 50, 50);

            // this
            btnAddCreditor.FlatAppearance.BorderColor = Color.FromArgb(0, 208, 255);
            btnAddCreditor.BackColor = Color.FromArgb(0, 208, 255);
        }

        private bool ValidateInputFields()
        {
            //
            // Име - Name (Required)
            //
            Regex regex = new Regex("^[a-zA-Z0-9 ]*$");

            if (!regex.IsMatch(addNameTextBox.Text))
            {
                MessageBox.Show(InvalidNameErrorMessage,
                    ErrorMessageBoxCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
            else if (string.IsNullOrEmpty(addNameTextBox.Text) || string.IsNullOrWhiteSpace(addNameTextBox.Text))
            {
                MessageBox.Show(MissingNameErrorMessage,
                    ErrorMessageBoxCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Тел № - PhoneNumber
            //
            regex = new Regex("^[+0-9-() ]*$");

            if (!regex.IsMatch(addPhoneTextBox.Text))
            {
                MessageBox.Show(InvalidPhoneNumberErrorMessage,
                    ErrorMessageBoxCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Имейл - Email
            //
            regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

            if (!regex.IsMatch(addEmailTextBox.Text.Trim()) && !string.IsNullOrEmpty(addEmailTextBox.Text.Trim()))
            {
                MessageBox.Show(InvalidEmailErrorMessage,
                    ErrorMessageBoxCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Фейсбук - Facebook
            //
            regex = new Regex("^[A-z ]*$");

            if (!regex.IsMatch(addFacebookTextBox.Text))
            {
                MessageBox.Show(InvalidFacebookErrorMessage,
                    ErrorMessageBoxCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            //
            // Количество - Amount (Required)
            //
            regex = new Regex("^[0-9]+([.,][0-9]{1,2})?$");
            decimal amount;

            if (regex.IsMatch(addAmountTextBox.Text.Trim()))
            {
                amount = decimal.Parse(addAmountTextBox.Text);

                if (amount < 0.01m || amount > 4294967295m)
                {
                    MessageBox.Show(string.Format(InvalidAmountInterval, MinAmountValue, MaxAmountValue),
                           ErrorMessageBoxCaption,
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Error);

                    return false;
                }
            }
            else if (string.IsNullOrEmpty(addAmountTextBox.Text) || string.IsNullOrWhiteSpace(addAmountTextBox.Text))
            {
                MessageBox.Show(MissingAmountErrorMessage,
                    ErrorMessageBoxCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
            else
            {
                MessageBox.Show(InvalidAmountErrorMessage,
                    ErrorMessageBoxCaption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            return true;
        }

        private void WarnUserOnExit(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show($"Данните няма да бъдат запазени.{Environment.NewLine}Наистина ли искате да излезете?",
                   ExitMessageBoxCaption,
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                // Deattaching the FormClosing event handler method so it doesn't glitch.
                // This is done before every this.Close(); method to avoid potential bugs.
                // In this case only the button Cancel and the EXIT button.
                this.FormClosing -= WarnUserOnExit;
                this.Close();
            }
            else if (dialog == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
