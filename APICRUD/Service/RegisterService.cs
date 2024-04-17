using APICRUD.Context;
using APICRUD.Entities;
using APICRUD.Model;
using Microsoft.EntityFrameworkCore;

namespace APICRUD.Service
{
    public interface IRegisterService
    {
        Task<ServiceResult<object>> Add(RegisterModel register);
        Task<ServiceResult<List<Register>>> Get();
        Task<ServiceResult<object>> Update(RegisterModel register, int id);
        Task<ServiceResult<object>> Delete(int id);
    }
    public class RegisterService : IRegisterService
    {
        private readonly ApplicationDbContext _context;
        public RegisterService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<object>> Add(RegisterModel register)
        {
            var result = new ServiceResult<object>();
            try
            {
                var info = new Register
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    FullName = register.FirstName + " " + register.LastName,
                    Mobile = register.Contact,
                    Email = register.Email,
                    Address = register.Address,
                    Gender = register.Gender,
                    Status = true
                };

                await _context.Registers.AddAsync(info);
                await _context.SaveChangesAsync();

                result.Entity = true;
                result.SuccessMessage = "Successfully inserted";
            }
            catch (Exception e)
            {
                SerilogLogger.Error(e, "Error with register user!");
            }

            return result;
        }

        public async Task<ServiceResult<List<Register>>> Get()
        {
            var result = new ServiceResult<List<Register>>();
            try
            {
                var item = await _context.Registers.Where(x => x.Status).ToListAsync();

                if (!item.Any())
                {
                    result.AddError("No active user found!");
                }

                result.Entity = item;
            }
            catch (Exception e)
            {
                SerilogLogger.Error(e, "Error with getting user details!");
            }

            return result;
        }

        public async Task<ServiceResult<object>> Update(RegisterModel register, int id)
        {
            var result = new ServiceResult<object>();
            try
            {
                var item = await _context.Registers.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (item == null)
                {
                    result.AddError($"User not found, with id : {id}");
                    return result;
                }

                item.Address = register.Address;
                item.Mobile = register.Contact;
                item.Email = register.Email;

                _context.Registers.Update(item);
                _context.SaveChanges();

                result.SuccessMessage = "User updated! ";
            }
            catch (Exception e)
            {
                SerilogLogger.Error(e, "Error with updating user!");
            }

            return result;
        }

        public async Task<ServiceResult<object>> Delete(int id)
        {
            var result = new ServiceResult<object>();
            try
            {
                var item = await _context.Registers.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (item == null)
                {
                    result.AddError($"User not found, with id : {id}");
                    return result;
                }

                item.Status = false;

                _context.Registers.Update(item);
                _context.SaveChanges();

                result.SuccessMessage = "User deleted! ";
            }
            catch (Exception e)
            {
                SerilogLogger.Error(e, "Error with deleting user!");
            }

            return result;
        }
    }
}
