using ExcelToDatabase.Data;
using ExcelToDatabase.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using ExcelToDatabase.Session;


namespace ExcelToDatabase.Controllers
{
    public class DataController : Controller
    {
        static DataController()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        private readonly ApplicationDBContext _context;

        public DataController(ApplicationDBContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Save()
        {

            List<CustomerViewModel> customers = HttpContext.Session.MyNewGetObject<List<CustomerViewModel>>("Users");
            
            List<CustomerData> allData = _context.customersdata.ToList();

            // List<CustomerData> unique = customers.Except(allData).ToList();

            var dataPhoneNumbers = allData.Select(d => d.PHONENUMBER);

            // Filter out customers whose phone numbers exist in allData
            List<CustomerViewModel> uniqueCustomers = customers.Where(c => !dataPhoneNumbers.Contains(c.PHONENUMBER)).ToList();

            if (uniqueCustomers != null)
            {
                foreach (var customer in uniqueCustomers)
                {
                    {
                        _context.customersdata.Add(new CustomerData
                        {

                            NAME = customer.NAME,
                            PHONENUMBER = customer.PHONENUMBER,
                            TAGS = customer.TAGS,
                            AGENTPHONENUMBER = customer.AGENTPHONENUMBER,
                            CUSTOMERDATECREATED = customer.CUSTOMERDATECREATED,
                            SOURCE = customer.SOURCE,
                            CUSTOMERBLOCKEDSTATUS = customer.CUSTOMERBLOCKEDSTATUS,
                            LASTTEMPLATESENTAT = customer.LASTTEMPLATESENTAT,
                            FIRSTMESSAGERECEIVEDAT = customer.FIRSTMESSAGERECEIVEDAT,
                            FIRSTMESSAGESENTAT = customer.FIRSTMESSAGESENTAT,
                            WHATSAPPNAME = customer.WHATSAPPNAME,
                            OPTOUT = customer.OPTOUT,
                            LASTMESSAGESENTAT = customer.LASTMESSAGESENTAT,
                            CUSTOMERNAME = customer.CUSTOMERNAME,
                            EMAIL = customer.EMAIL,
                            CITY = customer.CITY,
                            COI = customer.COI,
                            RTI = customer.RTI,
                            LINKEDIN = customer.LINKEDIN

                        });
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("success");
            }
            else
            {
                return View();
            }
        }

        public IActionResult success()
        {
            return View();
        }

        public IActionResult UploadExcel(List<CustomerViewModel> customer = null)
        {
            customer = customer == null ? new List<CustomerViewModel>() : customer;
            return View(customer);
        }

        [HttpPost]
        public IActionResult UploadExcel(IFormFile file)
        {
           
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file");
            }

            List<CustomerViewModel> customers = ReadCustomersFromExcel(file);
            ViewData["customers"] = customers;
            HttpContext.Session.MyNewSetObject("Users", customers);

            return View(customers);

        }

        private List<CustomerViewModel> ReadCustomersFromExcel(IFormFile file)
        {
            List<CustomerViewModel> customers = new List<CustomerViewModel>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming data is on the first worksheet

                    int columnCount = worksheet.Dimension.Columns;
                    int headerRow = 1; // Assuming the header row is the first row

                    for (int row = headerRow + 1; row <= worksheet.Dimension.Rows; row++)
                    {
                        CustomerViewModel customerModel = new CustomerViewModel();

                        for (int col = 1; col <= columnCount; col++)
                        {
                            string header = worksheet.Cells[headerRow, col].Value?.ToString();
                            string cellValue = worksheet.Cells[row, col].Value?.ToString();

                            if (header == null)
                                continue;

                            switch (header)
                            {
                                case "Name":
                                    customerModel.NAME = cellValue;
                                    break;
                                case "Phone number":
                                    if (decimal.TryParse(cellValue, out decimal phoneNumber))
                                    {
                                        customerModel.PHONENUMBER = phoneNumber;
                                    }
                                    break;
                                case "Tags":
                                    customerModel.TAGS = cellValue;
                                    break;
                                case "Agent Phone Number":
                                    if (decimal.TryParse(cellValue, out decimal agentPhoneNumber))
                                    {
                                        customerModel.AGENTPHONENUMBER = agentPhoneNumber;
                                    }
                                    break;
                                case "Customer Date Created":
                                    if (DateTime.TryParse(cellValue, out DateTime customerDateCreated))
                                    {
                                        customerModel.CUSTOMERDATECREATED = customerDateCreated;
                                    }
                                    break;
                                case "Source":
                                    customerModel.SOURCE = cellValue;
                                    break;
                                case "Customer blocked status":
                                    customerModel.CUSTOMERBLOCKEDSTATUS = cellValue;
                                    break;
                                case "Last template sent at":
                                    if (DateTime.TryParse(cellValue, out DateTime lastTemplateSentAt))
                                    {
                                        customerModel.LASTTEMPLATESENTAT = lastTemplateSentAt;
                                    }
                                    break;
                                case "First message received at":
                                    if (DateTime.TryParse(cellValue, out DateTime firstMessageReceivedAt))
                                    {
                                        customerModel.FIRSTMESSAGERECEIVEDAT = firstMessageReceivedAt;
                                    }
                                    break;
                                case "First message sent at":
                                    if (DateTime.TryParse(cellValue, out DateTime firstMessagesentAt))
                                    {
                                        customerModel.FIRSTMESSAGESENTAT = firstMessagesentAt;
                                    }
                                    break;
                                case "Whatsapp Name":
                                    customerModel.WHATSAPPNAME = cellValue;
                                    break;
                                case "Opt out":
                                    customerModel.OPTOUT = cellValue;
                                    break;
                                case "Last message sent at":
                                    if (DateTime.TryParse(cellValue, out DateTime lastMessageSentAt))
                                    {
                                        customerModel.LASTMESSAGESENTAT = lastMessageSentAt;
                                    }
                                    break;
                                //case "Last message sent at":
                                //customerModel.LASTMESSAGESENTAT = Convert.ToDateTime(cellValue); ;
                                //    break;
                                case "customerName":
                                    customerModel.CUSTOMERNAME = cellValue;
                                    break;
                                case "Email":
                                    customerModel.EMAIL = cellValue;
                                    break;
                                case "City":
                                    customerModel.CITY = cellValue;
                                    break;
                                case "COI":
                                    customerModel.COI = cellValue;
                                    break;
                                case "RTI":
                                    customerModel.RTI = cellValue;
                                    break;
                                case "LinkedIn":
                                    customerModel.LINKEDIN = cellValue;
                                    break;
                                    // Add cases for other headers as needed
                            }
                        }

                        customers.Add(customerModel);
                    }
                }
            }
            return customers;
        }
        
        [HttpPost]
        public IActionResult DuplicateRecords()
        {
            List<CustomerViewModel> customers = HttpContext.Session.MyNewGetObject<List<CustomerViewModel>>("Users");

            List<CustomerData> allData = _context.customersdata.ToList();

            

            var dataPhoneNumbers = allData.Select(d => d.PHONENUMBER);

          
            List<CustomerViewModel> DuplicateRecords = customers.Where(c => dataPhoneNumbers.Contains(c.PHONENUMBER)).ToList();
           
            return PartialView("Views/Data/_Duplicates.cshtml", DuplicateRecords);
        }
        [HttpPost]
        public IActionResult UniqueRecords()
        {
            List<CustomerViewModel> customers = HttpContext.Session.MyNewGetObject<List<CustomerViewModel>>("Users");

            List<CustomerData> allData = _context.customersdata.ToList();



            var dataPhoneNumbers = allData.Select(d => d.PHONENUMBER);

            List<CustomerViewModel> uniqueCustomers = customers.Where(c => !dataPhoneNumbers.Contains(c.PHONENUMBER)).ToList();
            if (uniqueCustomers.Any())
            {
                // Unique customers found, return PartialView with the data
                return PartialView("Views/Data/_UniqueRecords.cshtml", uniqueCustomers);
            }
            else
            {
                // No unique customers found, return an error or display a message
                return PartialView("Views/Data/_nouniquerecords.cshtml");
            }


        }

    }

}



