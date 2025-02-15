﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PhoneBase.ContextFolder;
using PhoneBase.Models;

namespace PhoneBase.Controllers
{
    public class PhonesController : Controller
    {
        private List<PhoneRecord> phoneRecords = new List<PhoneRecord>();
        private List<User> users = new List<User>();

        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        private bool isPhoneBaseLoaded = false;
        private bool isUsersLoaded = false;

        [HttpPost]
        public IActionResult AddUserAccount(string Login, string Password)
        {
            return Redirect("/");
        }

        public IActionResult DeleteUserAccount(string Login)
        {
            return Redirect("/");
        }


        public List<PhoneRecord> GetPhoneRecords()
        {
            return phoneRecords;
        }

        public IActionResult Index()
        {
            LoadPhoneBase();
            return View(phoneRecords);
        }

        public IActionResult PhoneRecordDetails(int ListIndex)
        {
            //LoadUsers();
            LoadPhoneBase();
            return View(phoneRecords[ListIndex]);
        }

        public IActionResult AddPhoneRecord()
        {
            //LoadUsers();
            LoadPhoneBase();
            return View();
        }

        [HttpPost]
        public IActionResult AddNewRecord(
            string Surname,
            string Name,
            string Patronymic,
            string Number,
            string Address,
            string Description)            
        {
            LoadPhoneBase();
            AddPhoneRecord(new PhoneRecord(phoneRecords.Count, Surname, Name, Patronymic, Number, Address, Description));
            return Redirect("/");
        }

        [HttpPost]
        public IActionResult EditRecord(
            int ID,
            string Surname,
            string Name,
            string Patronymic,
            string Number,
            string Address,
            string Description)
        {
            EditPhoneRecord(new PhoneRecord(ID, Surname, Name, Patronymic, Number, Address, Description));
            return Redirect("/");
        }

        public IActionResult DeleteRecord(int ID)
        {
            DeletePhoneRecord(ID);
            return Redirect("/");
        }

        private void LoadPhoneBase()
        {
            if (!isPhoneBaseLoaded)
            {
                using (DataContext context = new DataContext())
                {
                    var PhonesRequest = from phoneRecord in context.PhoneRecords
                                        orderby phoneRecord.id
                                        select phoneRecord;

                    foreach (PhoneRecord phoneRecord in PhonesRequest)
                    {
                        phoneRecords.Add(phoneRecord);
                    }

                    context.Dispose();
                }
                isPhoneBaseLoaded = true;
            }
        }

        private void AddPhoneRecord(PhoneRecord PhoneRecord)
        {
            using (DataContext context = new DataContext())
            {
                context.PhoneRecords.Add(PhoneRecord);
                phoneRecords.Add(PhoneRecord);

                context.SaveChanges();
                context.Dispose();
            }
        }

        private void EditPhoneRecord(PhoneRecord PhoneRecord)
        {
            using (DataContext context = new DataContext())
            {
                PhoneRecord _phoneRecord = context.PhoneRecords.Where(ph => ph.id == PhoneRecord.id).FirstOrDefault();

                _phoneRecord.surname = PhoneRecord.surname;
                _phoneRecord.name = PhoneRecord.name;
                _phoneRecord.patronymic = PhoneRecord.patronymic;
                _phoneRecord.number = PhoneRecord.number;
                _phoneRecord.address = PhoneRecord.address;
                _phoneRecord.description = PhoneRecord.description;

                foreach (PhoneRecord _ph in phoneRecords)
                {
                    if (_ph.id == PhoneRecord.id)
                    {
                        _ph.surname = PhoneRecord.surname;
                        _ph.name = PhoneRecord.name;
                        _ph.patronymic = PhoneRecord.patronymic;
                        _ph.number = PhoneRecord.number;
                        _ph.address = PhoneRecord.address;
                        _ph.description = PhoneRecord.description;
                        break;
                    }
                }

                context.SaveChanges();
                context.Dispose();
            }
        }

        private void DeletePhoneRecord(int ID)
        {
            using (DataContext context = new DataContext())
            {
                PhoneRecord _phoneRecord = context.PhoneRecords.Where(ph => ph.id == ID).FirstOrDefault();

                context.PhoneRecords.Remove(_phoneRecord);
                phoneRecords.Remove(_phoneRecord);

                context.SaveChanges();
                context.Dispose();
            }
        }
    }
}