﻿using System.Diagnostics;
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
                Console.WriteLine("L. List Users");
                Console.WriteLine("U. Update User");
                Console.WriteLine("D. Delete User");
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
                    case ConsoleKey.L:
                        await ListUsers();
                        break;
                    case ConsoleKey.U:
                        await UpdateUser();
                        break;
                    case ConsoleKey.D:
                        await DeleteUser();
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        #region // CRUD operations for projects
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
            var projectName = Console.ReadLine()!;
            if (string.IsNullOrEmpty(projectName.Trim()))
            {
                Console.WriteLine("Project name cannot be empty!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter new project description or leave empty to keep: ");
            var customerEmail = Console.ReadLine();

            Console.WriteLine("Enter new project start date or leave empty to keep(yyyy-MM-dd): ");
            var projectStartDateInput = Console.ReadLine()!;
            if (!DateValidationHelper.IsValidDate(projectStartDateInput))
            {
                Console.WriteLine("Invalid date format! Please use yyyy-MM-dd.");
                Console.ReadKey();
                return;
            }
            DateTime projectStartDate = DateTime.Parse(projectStartDateInput);
            if (!DateValidationHelper.IsFutureOrTodayDate(projectStartDate))
            {
                Console.WriteLine("Start date must be today or in the future!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter new project end date or leave empty to keep(yyyy-MM-dd): ");
            var projectEndDateInput = Console.ReadLine()!;
            DateTime? projectEndDate = null;
            if (!string.IsNullOrEmpty(projectEndDateInput))
            {
                if (!DateValidationHelper.IsValidDate(projectEndDateInput))
                {
                    Console.WriteLine("Invalid date format! Please use yyyy-MM-dd.");
                    Console.ReadKey();
                    return;
                }
                projectEndDate = DateTime.Parse(projectEndDateInput);
                if (!DateValidationHelper.IsEndDateAfterStartDate(projectStartDate, projectEndDate))
                {
                    Console.WriteLine("End date must be after start date!");
                    Console.ReadKey();
                    return;
                }
            }

            var statusTypes = await _statusService.GetStatusTypesAsync();
            Console.WriteLine($"Status types count: {statusTypes?.Count()}");

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
            var statusInput = Console.ReadLine()!;
            if (
                !int.TryParse(statusInput, out var statusIdInput)
                || !statusTypes.Any(s => s.Id == statusIdInput)
            )
            {
                Console.WriteLine("Invalid status Id!");
                Console.ReadKey();
                return;
            }

            var products = await _productService.GetProductsAsync();

            var selectedProduct = products.FirstOrDefault(p => p.Id == existingProject.Product.Id);
            if (selectedProduct == null)
            {
                Console.WriteLine("Invalid product selection!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Select a product or leave empty to keep: ");
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Status: {product.ProductName}");
            }

            Console.WriteLine("Enter product Id: ");
            var productIdInput = Console.ReadLine()!;
            if (
                !int.TryParse(productIdInput, out var productId)
                || !products.Any(p => p.Id == productId)
            )
            {
                Console.WriteLine("Invalid product Id!");
                Console.ReadKey();
                return;
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
            Console.WriteLine("Enter new existing project customer id or leave empty to keep: ");
            var customerIdInput = Console.ReadLine()!;

            #region // Validation generated by ChatGPT. If the user leaves customerIdInput empty, the existing customer ID is used. If the user provides a new ID, it is validated.
            int customerId;
            if (string.IsNullOrEmpty(customerIdInput))
            {
                customerId = existingProject.CustomerId;
            }
            else if (
                int.TryParse(customerIdInput, out int parsedCustomerId)
                && customers.Any(c => c.Id == parsedCustomerId)
            )
            {
                customerId = parsedCustomerId;
            }
            else
            {
                Console.WriteLine("Invalid Customer Id!");
                Console.ReadKey();
                return;
            }
            #endregion



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
            var userIdInput = Console.ReadLine()!;

            int userId;
            if (string.IsNullOrEmpty(userIdInput))
            {
                userId = existingProject.UserId;
            }
            else if (
                int.TryParse(userIdInput, out int parsedUserId)
                && users.Any(u => u.Id == parsedUserId)
            )
            {
                userId = parsedUserId;
            }
            else
            {
                Console.WriteLine("Invalid User Id!");
                Console.ReadKey();
                return;
            }

            var updatedProject = new ProjectEntity
            {
                Id = existingProject.Id,
                ProjectName = string.IsNullOrEmpty(projectName)
                    ? existingProject.ProjectName
                    : projectName,
                ProjectDescription = string.IsNullOrEmpty(customerEmail)
                    ? existingProject.ProjectDescription
                    : customerEmail,
                StartDate = string.IsNullOrEmpty(projectStartDateInput)
                    ? existingProject.StartDate
                    : DateTime.Parse(projectStartDateInput),
                EndDate = string.IsNullOrEmpty(projectEndDateInput)
                    ? existingProject.EndDate
                    : DateTime.Parse(projectEndDateInput),
                StatusId = statusIdInput,
                Product = selectedProduct,
                CustomerId = customerId,
                UserId = userId,
            };

            var result = await _projectService.UpdateProjectAsync(updatedProject);
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
                    Console.WriteLine($"Status: {project.Status}");
                    Console.WriteLine($"Customer: {project.Customer}");
                }
            }
            else
            {
                Console.WriteLine("No projects found!");
            }
            Console.ReadKey();
            return;
        }

        private async Task CreateProject()
        {
            Console.Clear();
            string projectName;
            Console.WriteLine("Enter project name:");
            do
            {
                projectName = Console.ReadLine()!;
                if (string.IsNullOrEmpty(projectName.Trim()))
                {
                    Console.WriteLine("Project name cannot be empty! Please try again: ");
                }
            } while (string.IsNullOrEmpty(projectName.Trim()));

            Console.WriteLine("Enter project description(optional):");
            var projectDescription = Console.ReadLine()!;

            Console.WriteLine("Enter project start date (yyyy-MM-dd):");
            DateTime projectStartDate = DateTime.MinValue;
            do
            {
                var projectStartDateInput = Console.ReadLine()!;
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
                }
            } while (!DateValidationHelper.IsValidDate(projectStartDate.ToString("yyy-MM-dd")));

            Console.WriteLine("(optional)Enter project end date (yyyy-MM-dd): ");
            DateTime? projectEndDate = null;
            while (true)
            {
                var projectEndDateInput = Console.ReadLine()!;
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

            Console.WriteLine("Select a project status: ");
            foreach (var status in statusTypes)
            {
                Console.WriteLine($"Id: {status.Id}");
                Console.WriteLine($"Name: {status.StatusName}");
            }

            Console.WriteLine("Enter status Id: ");
            var statusInput = Console.ReadLine()!;
            if (
                !int.TryParse(statusInput, out var statusIdInput)
                || !statusTypes.Any(s => s.Id == statusIdInput)
            )
            {
                Console.WriteLine("Invalid status Id!");
                Console.ReadKey();
                return;
            }

            var products = await _productService.GetProductsAsync();
            if (products == null || !products.Any())
            {
                Console.WriteLine("No products found!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Select a product: ");
            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Name: {product.ProductName}");
            }

            Console.WriteLine("Enter product Id: ");
            var productIdInput = Console.ReadLine()!;
            if (
                !int.TryParse(productIdInput, out var productId)
                || !products.Any(p => p.Id == productId)
            )
            {
                Console.WriteLine("Invalid product Id!");
                Console.ReadKey();
                return;
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

            Console.WriteLine("Enter User Id:");
            var userId = Console.ReadLine()!;
            if (!users.Any(u => u.Id.ToString() == userId))
            {
                Console.WriteLine("Invalid User Id!");
                Console.ReadKey();
                return;
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
            var customerId = Console.ReadLine()!;
            if (!customers.Any(c => c.Id.ToString() == customerId))
            {
                Console.WriteLine("Invalid Customer Id!");
                Console.ReadKey();
                return;
            }

            var projectRegistrationForm = new ProjectCreationForm
            {
                ProjectName = projectName,
                ProjectDescription = projectDescription,
                StartDate = projectStartDate,
                EndDate = projectEndDate,
                StatusId = statusIdInput,
                CustomerId = int.Parse(customerId),
                UserId = int.Parse(userId),
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
        }

        #endregion

        #region // CRUD operations for customers
        private async Task DeleteCustomer()
        {
            Console.Clear();

            var existingCustomer = await SelectCustomer();
            if (existingCustomer == null)
            {
                return;
            }

            Console.WriteLine("Are you sure you want to delete this customer? (Y/N)");
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
            }
            else
            {
                Console.WriteLine("Operation cancelled!");
            }
            Console.ReadKey();
        }

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
            var customerName = Console.ReadLine();

            Console.WriteLine("Enter new customer email or leave empty to keep: ");
            var customerEmail = Console.ReadLine();
            if (
                string.IsNullOrEmpty(customerEmail)
                || !EmailValidationHelper.IsValidEmail(customerEmail)
            )
            {
                Console.WriteLine("Invalid Email!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Enter new customer phone number or leave empty to keep: ");
            var customerPhone = Console.ReadLine();
            if (
                string.IsNullOrEmpty(customerPhone)
                || !PhoneValidationHelper.IsValidPhoneNumber(customerPhone)
            )
            {
                Console.WriteLine("Invalid phone number!");
                Console.ReadKey();
                return;
            }

            var updatedCustomer = new CustomerEntity
            {
                Id = existingCustomer.Id,
                CustomerName = string.IsNullOrEmpty(customerName)
                    ? existingCustomer.CustomerName
                    : customerName,
                CustomerEmail = string.IsNullOrEmpty(customerEmail)
                    ? existingCustomer.CustomerEmail
                    : customerEmail,
                CustomerPhone = string.IsNullOrEmpty(customerPhone)
                    ? existingCustomer.CustomerPhone
                    : customerPhone,
            };

            var result = await _customerService.UpdateCustomerAsync(updatedCustomer);
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

        private async Task RegisterCustomer()
        {
            Console.Clear();
            Console.WriteLine("Enter Customer Name:");
            var customerName = Console.ReadLine()!;
            Console.WriteLine("Enter Customer Email:");
            var customerEmail = Console.ReadLine()!;
            if (!EmailValidationHelper.IsValidEmail(customerEmail))
            {
                Console.WriteLine("Invalid Email!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Enter Customer Phone:");
            var customerPhone = Console.ReadLine()!;
            if (!PhoneValidationHelper.IsValidPhoneNumber(customerPhone))
            {
                Console.WriteLine("Invalid phone number!");
                Console.ReadKey();
                return;
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
        }

        #endregion

        #region // CRUD operations for users

        private async Task DeleteUser()
        {
            Console.Clear();

            var existingUser = await SelectUser();
            if (existingUser == null)
            {
                return;
            }

            Console.WriteLine("Are you sure you want to delete this user? (Y/N)");
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
            }
            else
            {
                Console.WriteLine("Operation cancelled!");
            }
        }

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
            var userFirstName = Console.ReadLine();
            Console.WriteLine("Enter new user last name or leave empty to keep: ");
            var userLastName = Console.ReadLine();
            Console.WriteLine("Enter new user email or leave empty to keep: ");
            var userEmail = Console.ReadLine();
            Console.WriteLine("Enter new user phone number or leave empty to keep: ");
            var userPhone = Console.ReadLine();

            var updatedUser = new UserEntity
            {
                Id = existingUser.Id,
                FirstName = string.IsNullOrEmpty(userFirstName)
                    ? existingUser.FirstName
                    : userFirstName,
                LastName = string.IsNullOrEmpty(userLastName)
                    ? existingUser.LastName
                    : userLastName,
                Email = string.IsNullOrEmpty(userEmail) ? existingUser.Email : userEmail,
                PhoneNumber = string.IsNullOrEmpty(userPhone)
                    ? existingUser.PhoneNumber
                    : userPhone,
            };

            var result = await _userService.UpdateUserAsync(updatedUser);
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

        private async Task RegisterUser()
        {
            Console.Clear();
            Console.WriteLine("Enter first name:");
            var userFirstName = Console.ReadLine()!;
            Console.WriteLine("Enter last name:");
            var userLastName = Console.ReadLine()!;
            Console.WriteLine("Enter email:");
            var userEmail = Console.ReadLine()!;
            Console.WriteLine("Enter phone number:");
            var userPhone = Console.ReadLine()!;

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
            var customerIdInput = Console.ReadLine();

            if (!int.TryParse(customerIdInput, out int customerId))
            {
                Console.WriteLine("Invalid Id!");
                Console.ReadKey();
                return null;
            }

            var existingCustomer = customers.FirstOrDefault(c => c.Id == customerId);

            if (existingCustomer == null)
            {
                Console.WriteLine("Customer not found!");
                Console.ReadKey();
                return null;
            }

            await _customerService.UpdateCustomerAsync(existingCustomer);

            return existingCustomer;
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
                Console.WriteLine($"Status: {project.Status}");
                Console.WriteLine($"Customer: {project.Customer}");
                Console.WriteLine($"User: {project.User}");
            }
            Console.WriteLine("Enter Project Id:");
            var projectIdInput = Console.ReadLine();

            if (string.IsNullOrEmpty(projectIdInput))
            {
                Console.WriteLine("Invalid Id!");
                Console.ReadKey();
                return null;
            }

            var existingProject = projects.FirstOrDefault(p => p.Id == projectIdInput);

            if (existingProject == null)
            {
                Console.WriteLine("Project not found!");
                Console.ReadKey();
                return null;
            }

            await _projectService.UpdateProjectAsync(existingProject);
            return existingProject;
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
            var userIdInput = Console.ReadLine();

            if (userIdInput == null)
            {
                Console.WriteLine("Invalid Id!");
                Console.ReadKey();
                return null;
            }

            var existingUser = users.FirstOrDefault(u => u.Id == int.Parse(userIdInput));
            if (existingUser == null)
            {
                Console.WriteLine("User not found!");
                Console.ReadKey();
                return null;
            }
            await _userService.UpdateUserAsync(existingUser);
            return existingUser;
        }
        #endregion
    }
}
