using System.Text;
using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Presentation.ConsoleApp.Interfaces;

namespace Presentation.ConsoleApp.Dialogs
{
    public class MenuDialog(
        ICustomerService customerService,
        IProjectService projectService,
        IUserService userService,
        IStatusService statusService,
        IProductService productService,
        ProjectFactory projectFactory,
        UserFactory userFactory
    ) : IMenuDialog
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly IProjectService _projectService = projectService;
        private readonly IUserService _userService = userService;
        private readonly IStatusService _statusService = statusService;
        private readonly IProductService _productService = productService;
        private readonly ProjectFactory _projectFactory = projectFactory;
        private readonly UserFactory _userFactory = userFactory;

        #region // Menu options
        public async Task MenuOptions()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1. Register Customer");
                Console.WriteLine("2. List Customers");
                Console.WriteLine("3. Update Customer");
                Console.WriteLine("4. Delete Customer");
                Console.WriteLine("5. Create Project");
                Console.WriteLine("6. List Projects");
                Console.WriteLine("7. Update Project");
                Console.WriteLine("8. Delete Project");
                Console.WriteLine("9. Register User");
                Console.WriteLine("F1. List Users");
                Console.WriteLine("F2. Update User");
                Console.WriteLine("F3. Delete User");
                Console.WriteLine("F4. Register Product");
                Console.WriteLine("F5. List Products");
                Console.WriteLine("F6. Update Product");
                Console.WriteLine("F7. Delete Product");
                Console.WriteLine("Esc. Exit");
                var key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.D1:
                        await RegisterCustomer();
                        break;
                    case ConsoleKey.D2:
                        await ListCustomers();
                        break;
                    case ConsoleKey.D3:
                        await UpdateCustomer();
                        break;
                    case ConsoleKey.D4:
                        await DeleteCustomer();
                        break;
                    case ConsoleKey.D5:
                        await CreateProject();
                        break;
                    case ConsoleKey.D6:
                        await ListProjects();
                        break;
                    case ConsoleKey.D7:
                        await UpdateProject();
                        break;
                    case ConsoleKey.D8:
                        await DeleteProject();
                        break;
                    case ConsoleKey.D9:
                        await RegisterUser();
                        break;
                    case ConsoleKey.F1:
                        await ListUsers();
                        break;
                    case ConsoleKey.F2:
                        await UpdateUser();
                        break;
                    case ConsoleKey.F3:
                        await DeleteUser();
                        break;
                    case ConsoleKey.F4:
                        await RegisterProduct();
                        break;
                    case ConsoleKey.F5:
                        await ListProducts();
                        break;
                    case ConsoleKey.F6:
                        await UpdateProduct();
                        break;
                    case ConsoleKey.F7:
                        await DeleteProduct();
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region // CRUD operations for projects
        // Create
        private async Task CreateProject()
        {
            Console.Clear();
            string projectName;
            Console.WriteLine("Enter project name:");
            do
            {
                projectName = ReadInputWithEscape()!;
                if (projectName == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(projectName.Trim()))
                {
                    Console.WriteLine("Project name cannot be empty! Please try again: ");
                }
            } while (string.IsNullOrEmpty(projectName.Trim()));

            Console.WriteLine("Enter project description(optional):");
            var projectDescription = ReadInputWithEscape()!;
            if (projectDescription == null)
            {
                return;
            }

            Console.WriteLine("Enter project start date (yyyy-MM-dd):");
            DateTime projectStartDate = DateTime.MinValue;
            while (true)
            {
                var projectStartDateInput = ReadInputWithEscape()!;
                if (projectStartDateInput == null)
                {
                    return;
                }
                if (!DateValidationHelper.IsValidDate(projectStartDateInput))
                {
                    Console.WriteLine("Invalid date format! Please use yyyy-MM-dd: ");
                    continue;
                }
                projectStartDate = DateTime.Parse(projectStartDateInput);
                if (!DateValidationHelper.IsFutureOrTodayDate(projectStartDate))
                {
                    Console.WriteLine(
                        "Start date must be today or in the future! Please try again: "
                    );
                    continue;
                }
                break;
            }

            Console.WriteLine("(optional)Enter project end date (yyyy-MM-dd): ");
            DateTime? projectEndDate = null;
            while (true)
            {
                var projectEndDateInput = ReadInputWithEscape()!;
                if (projectEndDateInput == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(projectEndDateInput))
                {
                    break;
                }
                if (!DateValidationHelper.IsValidDate(projectEndDateInput))
                {
                    Console.WriteLine("Invalid date format! Please use yyyy-MM-dd: ");
                    continue;
                }
                projectEndDate = DateTime.Parse(projectEndDateInput);
                if (!DateValidationHelper.IsEndDateAfterStartDate(projectStartDate, projectEndDate))
                {
                    Console.WriteLine("End date must be after start date!");
                    continue;
                }
                break;
            }

            var statusTypes = await _statusService.GetStatusTypesAsync();

            if (statusTypes == null || !statusTypes.Any())
            {
                Console.WriteLine("No status types found!");
                Console.ReadKey();
                return;
            }

            int statusIdInput;
            Console.WriteLine("Select a project status: ");

            foreach (var status in statusTypes)
            {
                Console.WriteLine($"Id: {status.Id}");
                Console.WriteLine($"Status: {status.StatusName}");
            }

            Console.WriteLine("Enter status Id: ");
            while (true)
            {
                var statusInput = ReadInputWithEscape()!;
                if (statusInput == null)
                {
                    return;
                }
                if (
                    int.TryParse(statusInput, out statusIdInput)
                    && statusTypes.Any(s => s.Id == statusIdInput)
                    && statusIdInput >= 1
                    && statusIdInput <= 3
                )
                {
                    break;
                }
                Console.WriteLine("Invalid status Id! Please try again: ");
            }

            var products = await _productService.GetProductsAsync();
            if (products == null || !products.Any())
            {
                Console.WriteLine("No products found! Press any key to return to main menu");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Select a product/service: ");
            int productId;
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Name: {product.ProductName}");
            }

            Console.WriteLine("Enter product Id: ");
            while (true)
            {
                var productIdInput = ReadInputWithEscape()!;

                if (
                    !int.TryParse(productIdInput, out productId)
                    || !products.Any(p => p.Id == productId)
                )
                {
                    Console.WriteLine("Invalid product Id! Please try again: ");
                    continue;
                }
                break;
            }

            var users = await _userService.GetUsersAsync();
            if (users == null || !users.Any())
            {
                Console.WriteLine("No users found!");
                Console.ReadKey();
                return;
            }
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user.Id}");
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
            }

            Console.WriteLine("Enter user Id:");
            int userId;
            while (true)
            {
                var userIdInput = ReadInputWithEscape()!;
                if (userIdInput == null)
                {
                    return;
                }
                if (int.TryParse(userIdInput, out userId) && users.Any(u => u.Id == userId))
                {
                    break;
                }
                {
                    Console.WriteLine("Invalid user Id! Please try again: ");
                }
            }

            var customers = await _customerService.GetCustomersAsync();
            if (customers == null || !customers.Any())
            {
                Console.WriteLine("No customers found!");
                Console.ReadKey();
                return;
            }
            foreach (var customer in customers)
            {
                Console.WriteLine($"Id: {customer.Id}");
                Console.WriteLine($"Name: {customer.CustomerName}");
            }

            Console.WriteLine("Enter Customer Id:");
            int customerId;
            while (true)
            {
                var customerIdInput = ReadInputWithEscape()!;
                if (customerIdInput == null)
                {
                    return;
                }
                if (
                    int.TryParse(customerIdInput, out customerId)
                    && customers.Any(c => c.Id == customerId)
                )
                {
                    break;
                }

                Console.WriteLine("Invalid Customer Id! Please try again: ");
            }

            var projectRegistrationForm = new ProjectCreationForm
            {
                ProjectName = projectName,
                ProjectDescription = projectDescription,
                StartDate = projectStartDate,
                EndDate = projectEndDate,
                StatusId = statusIdInput,
                CustomerId = customerId,
                ProductId = productId,
                UserId = userId,
            };

            var newProject = await _projectFactory.CreateAsync(projectRegistrationForm);

            var result = await _projectService.CreateProjectAsync(newProject);

            if (result != null)
            {
                Console.WriteLine("Project created successfully!");
            }
            else
            {
                Console.WriteLine("Failed to create project!");
            }
            Console.ReadLine();
        }

        // Read
        private async Task ListProjects()
        {
            Console.Clear();
            var projects = await _projectService.GetProjectsAsync();
            if (projects.Any() && projects != null)
            {
                foreach (var project in projects)
                {
                    Console.WriteLine($"Name: {project.ProjectName}");
                    Console.WriteLine($"Description: {project.ProjectDescription}");
                    Console.WriteLine($"Start date: {project.StartDate}");
                    Console.WriteLine($"End date: {project.EndDate}");
                    Console.WriteLine($"Status: {project.Status.StatusName}");
                    Console.WriteLine($"Customer: {project.Customer.CustomerName}");
                    Console.WriteLine($"Product: {project.Product.ProductName}");
                    Console.WriteLine($"User: {project.User.FirstName} {project.User.LastName}");
                }
            }
            else
            {
                Console.WriteLine("No projects found!");
            }
            Console.ReadKey();
            return;
        }

        // Update
        private async Task UpdateProject()
        {
            Console.Clear();
            var existingProject = await SelectProject();
            if (existingProject == null)
            {
                return;
            }

            Console.WriteLine($"Update project: {existingProject.ProjectName}");

            Console.WriteLine("Enter new project name or leave empty to keep: ");
            var projectName = ReadInputWithEscape()!;
            if (projectName == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(projectName))
            {
                projectName = existingProject.ProjectName;
            }
            existingProject.ProjectName = projectName;

            Console.WriteLine("Enter new project description or leave empty to keep: ");
            var projectDescription = ReadInputWithEscape()!;
            if (projectDescription == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(projectDescription))
            {
                projectDescription = existingProject.ProjectDescription;
            }
            existingProject.ProjectDescription = projectDescription;

            Console.WriteLine("Enter new project start date or leave empty to keep(yyyy-MM-dd): ");
            DateTime projectStartDate;
            string projectStartDateInput;
            while (true)
            {
                projectStartDateInput = ReadInputWithEscape()!;
                if (projectStartDateInput == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(projectStartDateInput))
                {
                    projectStartDate = existingProject.StartDate;
                    break;
                }

                if (!DateValidationHelper.IsValidDate(projectStartDateInput))
                {
                    Console.WriteLine(
                        "Invalid date format! Please use yyyy-MM-dd or leave empty: "
                    );
                    continue;
                }
                projectStartDate = DateTime.Parse(projectStartDateInput);
                if (!DateValidationHelper.IsFutureOrTodayDate(projectStartDate))
                {
                    Console.WriteLine("Start date must be today or in the future!: ");
                    continue;
                }
                break;
            }
            existingProject.StartDate = projectStartDate;

            Console.WriteLine("Enter new project end date or leave empty to keep(yyyy-MM-dd): ");
            string projectEndDateInput;
            DateTime? projectEndDate = existingProject.EndDate;

            while (true)
            {
                projectEndDateInput = ReadInputWithEscape()!;
                if (projectEndDateInput == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(projectEndDateInput))
                {
                    break;
                }

                if (!DateValidationHelper.IsValidDate(projectEndDateInput))
                {
                    Console.WriteLine("Invalid date format! Please use yyyy-MM-d or leave empty: ");
                    continue;
                }
                projectEndDate = DateTime.Parse(projectEndDateInput);

                if (!DateValidationHelper.IsEndDateAfterStartDate(projectStartDate, projectEndDate))
                {
                    Console.WriteLine("End date must be after start date!: ");
                    continue;
                }
                break;
            }
            existingProject.EndDate = projectEndDate;

            var statusTypes = await _statusService.GetStatusTypesAsync();
            if (statusTypes == null || !statusTypes.Any())
            {
                Console.WriteLine("No status types found!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Select a project status or leave empty to keep: ");
            foreach (var status in statusTypes)
            {
                Console.WriteLine($"Id: {status.Id}");
                Console.WriteLine($"Status: {status.StatusName}");
            }

            Console.WriteLine("Enter status Id: ");
            string statusInput;
            var selectedStatus = existingProject.Status;
            while (true)
            {
                statusInput = ReadInputWithEscape()!;
                if (statusInput == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(statusInput))
                {
                    break;
                }
                if (
                    int.TryParse(statusInput, out var statusIdInput)
                    && statusTypes.Any(s => s.Id == statusIdInput)
                )
                {
                    selectedStatus = statusTypes.First(s => s.Id == statusIdInput);
                    break;
                }
                Console.WriteLine(
                    "Invalid status id! please chose a valid status or leave empty: "
                );
            }
            existingProject.Status = selectedStatus;

            var products = await _productService.GetProductsAsync();
            if (products == null || !products.Any())
            {
                Console.WriteLine("No products found!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Select a product or leave empty to keep: ");
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Status: {product.ProductName}");
            }

            var selectedProduct = existingProject.Product;
            string productIdInput;
            Console.WriteLine("Enter product Id: ");

            while (true)
            {
                productIdInput = ReadInputWithEscape()!;
                if (productIdInput == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(productIdInput))
                {
                    break;
                }
                if (
                    int.TryParse(productIdInput, out var productId)
                    && products.Any(p => p.Id == productId)
                )
                {
                    selectedProduct = products.First(p => p.Id == productId);
                    break;
                }
                Console.WriteLine("Invalid product id! Please try again or leave empty: ");
            }
            existingProject.Product = selectedProduct;

            var customers = await _customerService.GetCustomersAsync();
            if (customers == null || !customers.Any())
            {
                Console.WriteLine("No customers found!");
                Console.ReadKey();
                return;
            }
            foreach (var customer in customers)
            {
                Console.WriteLine($"Id: {customer.Id}");
                Console.WriteLine($"Name: {customer.CustomerName}");
            }
            Console.WriteLine("Enter new existing project customer id or leave empty to keep: ");
            string customerIdInput;
            var selectedCustomer = existingProject.Customer;
            while (true)
            {
                customerIdInput = ReadInputWithEscape()!;
                if (customerIdInput == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(customerIdInput))
                {
                    break;
                }
                if (
                    int.TryParse(customerIdInput, out var customerId)
                    && customers.Any(c => c.Id == customerId)
                )
                {
                    selectedCustomer = customers.First(c => c.Id == customerId);
                    break;
                }
                Console.WriteLine("Invalid customer id! Please try again or leave empty: ");
            }
            existingProject.Customer = selectedCustomer;

            var users = await _userService.GetUsersAsync();
            if (users == null || !users.Any())
            {
                Console.WriteLine("No users found!");
                Console.ReadKey();
                return;
            }
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user.Id}");
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
            }
            Console.WriteLine("Enter new existing user id or leave empty to keep: ");
            string userIdInput;
            var selectedUser = existingProject.User;
            while (true)
            {
                userIdInput = ReadInputWithEscape()!;
                if (userIdInput == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(userIdInput))
                {
                    break;
                }
                if (int.TryParse(userIdInput, out var userId) && users.Any(u => u.Id == userId))
                {
                    selectedUser = users.FirstOrDefault(u => u.Id == userId);
                    break;
                }
                Console.WriteLine("Invalid user id! Please try again or leave empty: ");
            }
            existingProject.User =
                selectedUser ?? throw new InvalidOperationException("Selected user cannot be null");

            var result = await _projectService.UpdateProjectAsync(existingProject);
            if (result != null)
            {
                Console.WriteLine("Project updated successfully!");
            }
            else
            {
                Console.WriteLine("Failed to update project!");
            }
            Console.ReadKey();
        }

        // Delete
        private async Task DeleteProject()
        {
            Console.Clear();

            var existingProject = await SelectProject();
            if (existingProject == null)
            {
                return;
            }

            Console.WriteLine("Are you sure you want to delete this project? (Y/N)");
            var confirmation = Console.ReadLine();

            if (confirmation?.ToLower() == "y")
            {
                var deletedProject = await _projectService.DeleteProjectAsync(existingProject.Id);
                if (deletedProject != null)
                {
                    Console.WriteLine("Project deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to delete project!");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Operation canceled!");
            }
            Console.ReadKey();
        }

        #endregion

        #region // CRUD operations for customers
        // Create
        private async Task RegisterCustomer()
        {
            Console.Clear();
            Console.WriteLine("Enter Customer Name:");
            string customerName;
            while (true)
            {
                customerName = ReadInputWithEscape()!;
                if (customerName == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(customerName))
                {
                    Console.WriteLine("Customer name cannot be empty! Please try again:");
                    continue;
                }

                break;
            }

            Console.WriteLine("Enter Customer Email:");
            string customerEmail;
            while (true)
            {
                customerEmail = ReadInputWithEscape()!;
                if (customerEmail == null)
                {
                    return;
                }
                if (
                    string.IsNullOrEmpty(customerEmail)
                    || !EmailValidationHelper.IsValidEmail(customerEmail)
                )
                {
                    Console.WriteLine("Invalid Email! Please try again: ");
                    continue;
                }
                break;
            }

            Console.WriteLine("Enter Customer Phone:");
            string customerPhone;
            while (true)
            {
                customerPhone = ReadInputWithEscape()!;
                if (customerPhone == null)
                {
                    return;
                }
                if (
                    string.IsNullOrWhiteSpace(customerPhone)
                    || !PhoneValidationHelper.IsValidPhoneNumber(customerPhone)
                )
                {
                    Console.WriteLine("Invalid phone number! Please try again: ");
                    continue;
                }
                break;
            }

            var customerRegistrationForm = new CustomerRegistrationForm
            {
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerPhone = customerPhone,
            };

            var newCustomer = CustomerFactory.Create(customerRegistrationForm);

            var result = await _customerService.CreateCustomerAsync(newCustomer);

            if (result != null)
            {
                Console.WriteLine("Customer registered successfully!");
            }
            else
            {
                Console.WriteLine("Failed to register customer!");
            }
            Console.ReadKey();
        }

        // Read
        private async Task ListCustomers()
        {
            Console.Clear();
            var customers = await _customerService.GetCustomersAsync();
            if (customers.Any() && customers != null)
            {
                foreach (var customer in customers)
                {
                    Console.WriteLine($"Name: {customer.CustomerName}");
                    Console.WriteLine($"Email: {customer.CustomerEmail}");
                    Console.WriteLine($"Phone: {customer.CustomerPhone}");
                }
            }
            else
            {
                Console.WriteLine("No customers found!");
            }
            Console.ReadKey();
        }

        // Update
        private async Task UpdateCustomer()
        {
            Console.Clear();
            var existingCustomer = await SelectCustomer();
            if (existingCustomer == null)
            {
                return;
            }

            Console.WriteLine($"Update customer: {existingCustomer.CustomerName}");

            Console.WriteLine("Enter new customer name or leave empty to keep: ");
            var customerName = ReadInputWithEscape()!;
            if (customerName == null)
            {
                return;
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                existingCustomer.CustomerName = customerName;
            }

            Console.WriteLine("Enter new customer email or leave empty to keep: ");
            while (true)
            {
                var customerEmail = ReadInputWithEscape()!;
                if (customerEmail == null)
                {
                    return;
                }
                if (
                    string.IsNullOrEmpty(customerEmail)
                    || EmailValidationHelper.IsValidEmail(customerEmail)
                )
                {
                    if (!string.IsNullOrEmpty(customerEmail))
                    {
                        existingCustomer.CustomerEmail = customerEmail;
                    }
                    break;
                }
                Console.WriteLine("Invalid email! Please try again or leave empty to keep: ");
            }

            Console.WriteLine("Enter new customer phone number or leave empty to keep: ");
            while (true)
            {
                var customerPhone = ReadInputWithEscape()!;
                if (customerPhone == null)
                {
                    return;
                }
                if (
                    string.IsNullOrEmpty(customerPhone)
                    || PhoneValidationHelper.IsValidPhoneNumber(customerPhone)
                )
                {
                    if (!string.IsNullOrEmpty(customerPhone))
                    {
                        existingCustomer.CustomerPhone = customerPhone;
                    }
                    break;
                }
                Console.WriteLine(
                    "Invalid phone number! Please try again or leave empty to keep: "
                );
            }

            var result = await _customerService.UpdateCustomerAsync(existingCustomer);
            if (result != null)
            {
                Console.WriteLine("Customer updated successfully!");
            }
            else
            {
                Console.WriteLine("Failed to update customer!");
            }
            Console.ReadKey();
        }

        // Delete
        private async Task DeleteCustomer()
        {
            Console.Clear();

            var existingCustomer = await SelectCustomer();
            if (existingCustomer == null)
            {
                return;
            }

            Console.WriteLine(
                "Are you sure you want to delete this customer? Any associated projects will also get removed (Y/N)"
            );

            while (true)
            {
                var confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "y")
                {
                    var deletedCustomer = await _customerService.DeleteCustomerAsync(
                        existingCustomer.Id
                    );
                    if (deletedCustomer != null)
                    {
                        Console.WriteLine("Customer deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete customer!");
                    }
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("Operation canceled!");
                }
                Console.ReadKey();
            }
        }

        #endregion

        #region // CRUD operations for users
        // Create
        private async Task RegisterUser()
        {
            Console.Clear();
            string userFirstName;
            Console.WriteLine("Enter first name:");
            while (true)
            {
                userFirstName = ReadInputWithEscape()!;
                if (userFirstName == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(userFirstName))
                {
                    Console.WriteLine("First name cannot be empty! Please try again: ");
                    continue;
                }
                break;
            }

            string userLastName;
            Console.WriteLine("Enter last name:");
            while (true)
            {
                userLastName = ReadInputWithEscape()!;
                if (userLastName == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(userLastName))
                {
                    Console.WriteLine("Last name cannot be empty! Please try again: ");
                    continue;
                }
                break;
            }

            string userEmail;
            Console.WriteLine("Enter email:");
            while (true)
            {
                userEmail = ReadInputWithEscape()!;
                if (userEmail == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(userEmail))
                {
                    Console.WriteLine("Email cannot be empty! Please try again: ");
                    continue;
                }
                if (!EmailValidationHelper.IsValidEmail(userEmail))
                {
                    Console.WriteLine("Invalid email format! Please try again: ");
                    continue;
                }
                break;
            }

            string userPhone;
            Console.WriteLine("Enter phone number:");
            while (true)
            {
                userPhone = ReadInputWithEscape()!;
                if (userPhone == null)
                {
                    return;
                }
                if (
                    string.IsNullOrWhiteSpace(userPhone)
                    || !PhoneValidationHelper.IsValidPhoneNumber(userPhone)
                )
                {
                    Console.WriteLine("Invalid phone number format. Please try again: ");
                    continue;
                }
                break;
            }

            var userRegistrationForm = new UserRegistrationForm
            {
                FirstName = userFirstName,
                LastName = userLastName,
                Email = userEmail,
                PhoneNumber = userPhone,
            };

            var newUser = await _userFactory.CreateAsync(userRegistrationForm);

            var result = await _userService.CreateUserAsync(newUser);

            if (result != null)
            {
                Console.WriteLine("User registered successfully!");
            }
            else
            {
                Console.WriteLine("Failed to register user!");
            }
            Console.ReadKey();
        }

        // Read
        private async Task ListUsers()
        {
            Console.Clear();
            var users = await _userService.GetUsersAsync();
            if (users.Any() && users != null)
            {
                foreach (var user in users)
                {
                    Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
                    Console.WriteLine($"Email: {user.Email}");
                    Console.WriteLine($"Phone: {user.PhoneNumber}");
                }
            }
            else
            {
                Console.WriteLine("No users found!");
            }
            Console.ReadKey();
        }

        // Update
        private async Task UpdateUser()
        {
            Console.Clear();
            var existingUser = await SelectUser();
            if (existingUser == null)
            {
                return;
            }

            Console.WriteLine($"Update user: {existingUser.FirstName} {existingUser.LastName}");

            Console.WriteLine("Enter new user first name or leave empty to keep: ");
            while (true)
            {
                var firstNameInput = ReadInputWithEscape()!;
                if (firstNameInput == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(firstNameInput))
                {
                    break;
                }
                existingUser.FirstName = firstNameInput;
                break;
            }

            Console.WriteLine("Enter new user last name or leave empty to keep: ");
            while (true)
            {
                var lastNameInput = ReadInputWithEscape()!;
                if (lastNameInput == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(lastNameInput))
                {
                    break;
                }
                existingUser.LastName = lastNameInput;
                break;
            }

            Console.WriteLine("Enter new user email or leave empty to keep: ");
            while (true)
            {
                var emailInput = ReadInputWithEscape()!;
                if (emailInput == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(emailInput))
                {
                    break;
                }
                if (!EmailValidationHelper.IsValidEmail(emailInput))
                {
                    Console.WriteLine("Invalid Email! Please try again or leave empty to keep");
                    continue;
                }
                existingUser.Email = emailInput;
                break;
            }

            Console.WriteLine("Enter new user phone number or leave empty to keep: ");

            while (true)
            {
                var phoneInput = ReadInputWithEscape()!;
                if (phoneInput == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(phoneInput))
                {
                    break;
                }
                if (!PhoneValidationHelper.IsValidPhoneNumber(phoneInput))
                {
                    Console.WriteLine(
                        "Invalid phone number! Please try again or leave emtpy to keep: "
                    );
                    continue;
                }
                existingUser.PhoneNumber = phoneInput;
                break;
            }

            var result = await _userService.UpdateUserAsync(existingUser);
            if (result != null)
            {
                Console.WriteLine("User updated successfully!");
            }
            else
            {
                Console.WriteLine("Failed to update user!");
            }
            Console.ReadKey();
        }

        // Delete
        private async Task DeleteUser()
        {
            Console.Clear();

            var existingUser = await SelectUser();
            if (existingUser == null)
            {
                return;
            }

            Console.WriteLine(
                "Are you sure you want to delete this user? Any associated projects will also get removed (Y/N)"
            );

            while (true)
            {
                var confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "y")
                {
                    var deletedUser = await _userService.DeleteUserAsync(existingUser.Id);
                    if (deletedUser != null)
                    {
                        Console.WriteLine("User deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete user!");
                    }
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("Operation canceled!");
                    Console.ReadKey();
                    break;
                }
            }
        }

        #endregion

        #region // CRUD operations for products
        // Create
        private async Task RegisterProduct()
        {
            Console.Clear();
            string productName;
            Console.WriteLine("Enter Product Name:");

            while (true)
            {
                productName = ReadInputWithEscape()!;
                if (productName == null)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(productName))
                {
                    Console.WriteLine("Product name cannot be empty! Please try again:");
                }
                else
                {
                    break;
                }
            }

            decimal price;
            Console.WriteLine("Enter product price: ");

            while (true)
            {
                var priceInput = ReadInputWithEscape()!;
                if (priceInput == null)
                {
                    return;
                }
                if (!decimal.TryParse(priceInput, out price) || price <= 0)
                {
                    Console.WriteLine("Invalid price! Please enter a valid price:");
                }
                else
                {
                    break;
                }
            }

            var productRegistrationForm = new ProductRegistrationForm
            {
                ProductName = productName,
                Price = price,
            };

            try
            {
                var newProduct = ProductFactory.CreateProduct(
                    productRegistrationForm.ProductName,
                    productRegistrationForm.Price
                );

                var result = await _productService.CreateProductAsync(newProduct);

                if (result != null)
                {
                    Console.WriteLine("Product registered successfully!");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Failed to register product!");
                    Console.ReadKey();
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }

        // Read
        private async Task ListProducts()
        {
            Console.Clear();
            var products = await _productService.GetProductsAsync();

            if (!products.Any())
            {
                Console.WriteLine("No products found!");
            }
            else
            {
                Console.WriteLine("Product list:");
                foreach (var product in products)
                {
                    Console.WriteLine($"Name: {product.ProductName}");
                    Console.WriteLine($"Price: {product.Price}");
                }
            }
            Console.ReadKey();
        }

        // Update
        private async Task UpdateProduct()
        {
            var existingProduct = await SelectProduct();
            if (existingProduct == null)
            {
                return;
            }
            Console.WriteLine($"Update product: {existingProduct.ProductName}");

            Console.WriteLine("Enter new product name or leave empty to keep: ");
            while (true)
            {
                var productName = ReadInputWithEscape()!;
                if (productName == null)
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(productName))
                {
                    break;
                }
                else
                {
                    existingProduct.ProductName = productName;
                    break;
                }
            }

            Console.WriteLine("Enter new product price or leave empty to keep: ");
            while (true)
            {
                var priceInput = ReadInputWithEscape()!;
                if (priceInput == null)
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(priceInput))
                {
                    break;
                }

                if (decimal.TryParse(priceInput, out decimal price) && price > 0)
                {
                    existingProduct.Price = price;
                    break;
                }
                Console.WriteLine("Invalid price! Please try again or leave empty to keep: ");
            }

            var result = await _productService.UpdateProductAsync(existingProduct);
            if (result != null)
            {
                Console.WriteLine("Product updated successfully!");
            }
            else
            {
                Console.WriteLine("Failed to update product!");
            }
            Console.ReadKey();
        }

        // Delete
        private async Task DeleteProduct()
        {
            var existingProduct = await SelectProduct();
            if (existingProduct == null)
            {
                return;
            }

            Console.WriteLine(
                "Are you sure you want to delete this product? Any associated projects will also get removed (Y/N)"
            );

            while (true)
            {
                var confirmation = Console.ReadLine()!;
                if (confirmation?.ToLower() == "y")
                {
                    var deletedProduct = await _productService.DeleteProductAsync(
                        existingProduct.Id
                    );
                    if (deletedProduct != null)
                    {
                        Console.WriteLine("Product deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to delete product!");
                    }
                    Console.ReadKey();
                    break;
                }
                else if (confirmation?.ToLower() == "n")
                {
                    Console.WriteLine("Operation canceled!");
                    Console.ReadKey();
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid input! Please enter Y or N:");
                }
                Console.ReadKey();
            }
        }
        #endregion

        #region // Helper methods
        private async Task<CustomerEntity?> SelectCustomer()
        {
            Console.Clear();
            var customers = await _customerService.GetCustomersAsync();

            if (customers == null || !customers.Any())
            {
                Console.WriteLine("No customers found!");
                Console.ReadKey();
                return null;
            }
            Console.WriteLine("Customers:");
            foreach (var customer in customers)
            {
                Console.WriteLine($"Id: {customer.Id}");
                Console.WriteLine($"Name: {customer.CustomerName}");
                Console.WriteLine($"Email: {customer.CustomerEmail}");
                Console.WriteLine($"Phone: {customer.CustomerPhone}");
            }
            Console.WriteLine("Enter Customer Id:");
            while (true)
            {
                var customerIdInput = Console.ReadLine();

                if (int.TryParse(customerIdInput, out int customerId))
                {
                    var existingCustomer = customers.FirstOrDefault(c => c.Id == customerId);
                    if (existingCustomer != null)
                    {
                        return existingCustomer;
                    }
                    Console.WriteLine("Customer not found. Please try again: ");
                }
                else
                {
                    Console.WriteLine("Invalid Id! Please try again");
                }
            }
        }

        private async Task<ProjectEntity?> SelectProject()
        {
            Console.Clear();
            var projects = await _projectService.GetProjectsAsync();

            if (projects == null || !projects.Any())
            {
                Console.WriteLine("No projects found!");
                Console.ReadKey();
                return null;
            }
            Console.WriteLine("Projects:");
            foreach (var project in projects)
            {
                Console.WriteLine($"Id: {project.Id}");
                Console.WriteLine($"Name: {project.ProjectName}");
                Console.WriteLine($"Description: {project.ProjectDescription}");
                Console.WriteLine($"Start Date: {project.StartDate}");
                Console.WriteLine($"End Date: {project.EndDate}");
                Console.WriteLine($"Status: {project.Status.StatusName}");
                Console.WriteLine($"Customer: {project.Customer.CustomerName}");
                Console.WriteLine($"User: {project.User.FirstName} {project.User.LastName}");
                Console.WriteLine("----------------------------------");
            }
            Console.WriteLine("Enter Project Id:");

            while (true)
            {
                var projectIdInput = ReadInputWithEscape()!;
                if (projectIdInput == null)
                {
                    return null;
                }

                var existingProject = projects.FirstOrDefault(p => p.Id == projectIdInput);
                if (existingProject != null)
                {
                    return existingProject;
                }
                else
                {
                    Console.WriteLine("Project not found. Please try again: ");
                }
            }
        }

        private async Task<UserEntity?> SelectUser()
        {
            Console.Clear();
            var users = await _userService.GetUsersAsync();

            if (users == null || !users.Any())
            {
                Console.WriteLine("No users found!");
                Console.ReadKey();
                return null;
            }
            Console.WriteLine("Users:");
            foreach (var user in users)
            {
                Console.WriteLine($"Id: {user.Id}");
                Console.WriteLine($"Name: {user.FirstName} {user.LastName}");
            }
            Console.WriteLine("Enter User Id:");
            while (true)
            {
                var userIdInput = ReadInputWithEscape();
                if (userIdInput == null)
                {
                    return null;
                }

                if (int.TryParse(userIdInput, out int userId))
                {
                    var existingUser = users.FirstOrDefault(u => u.Id == userId);
                    if (existingUser != null)
                    {
                        return existingUser;
                    }
                    Console.WriteLine("User not found. Please try again: ");
                }
                else
                {
                    Console.WriteLine("Invalid Id! Please try again");
                }
            }
        }

        private async Task<ProductEntity?> SelectProduct()
        {
            Console.Clear();
            var products = await _productService.GetProductsAsync();

            if (products == null || !products.Any())
            {
                Console.WriteLine("No products found!");
                Console.ReadKey();
                return null;
            }
            Console.WriteLine("Products: ");
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Name: {product.ProductName}");
                Console.WriteLine($"Price: {product.Price}");
            }

            ProductEntity? existingProduct = null;

            Console.WriteLine("Enter products Id:");
            while (existingProduct == null)
            {
                var productIdInput = ReadInputWithEscape()!;
                if (productIdInput == null)
                {
                    return null;
                }

                if (!int.TryParse(productIdInput, out int productId))
                {
                    Console.WriteLine("Invalid Id! Please try again: ");
                    continue;
                }

                existingProduct = products.FirstOrDefault(c => c.Id == productId);

                if (existingProduct == null)
                {
                    Console.WriteLine("Product not found! Please try again: ");
                }
            }

            return existingProduct;
        }

        // Helper method generated by ChatGPT to read input from user with escape key
        private static string? ReadInputWithEscape()
        {
            StringBuilder input = new();

            while (true)
            {
                var key = Console.ReadKey(true); // Read user keypress (without displaying it automatically)

                if (key.Key == ConsoleKey.Escape)
                    return null; // Cancel operation
                if (key.Key == ConsoleKey.Enter)
                    break; // Accept input

                if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                {
                    input.Remove(input.Length - 1, 1); // Remove last character
                    Console.Write("\b \b"); // Erase character on screen
                    continue;
                }

                input.Append(key.KeyChar); // Append typed character
                Console.Write(key.KeyChar); // Display typed character
            }

            Console.WriteLine(); // Move to next line after Enter
            return input.ToString();
        }

        #endregion
    }
}
