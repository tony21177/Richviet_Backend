using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Frontend.DB.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Users.Domains.Users.Command.Adapter.Repositories
{
    public class UserCommandDbCommandRepository : IUserCommandRepository
    {
        private readonly GeneralContext _context;

        public UserCommandDbCommandRepository(GeneralContext context)
        {
            _context = context;
        }

        public void Modify(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
        }


        public int DeleteDraftUser()
        {
            int numberOfEffectRow = _context.User.Where(x => x.UserArc.KycStatus == 0).DeleteFromQuery();

            _context.SaveChanges();
            return numberOfEffectRow;
        }

        public int DeleteDraftUserAfterDays(int days)
        {
            DateTime now = DateTime.UtcNow;
            DateTime deletedDate = now.AddDays(-days);
            //List<User> userList = _context.User.Include(user => user.UserArc).Where(user => user.UserArc.KycStatus == 0 && System.Data.Entity.DbFunctions.DiffDays(user.UserArc.CreateTime, now) >= days).Include(user=>user.UserRegisterType).ToList();
            List<User> userList = _context.User.Include(user => user.UserArc).Where(user => user.UserArc.KycStatus == 0 && user.UserArc.CreateTime< deletedDate).Include(user => user.UserRegisterType).ToList();
            _context.User.RemoveRange(userList);

            _context.SaveChanges();
            return 0;
        }

        public int DeleteUser(long userId)
        {
            //List<RemitRecord> remitRecords = _context.RemitRecord.Include(recored => recored.RemitAdminReviewLog).Where(record => record.UserId == userId).ToList();
            //_context.RemitRecord.RemoveRange(remitRecords);
            //List<OftenBeneficiary> beneficiaries = _context.OftenBeneficiary.Where(beneficiary => beneficiary.UserId == userId).ToList();
            //_context.OftenBeneficiary.RemoveRange(beneficiaries);
            List<User> userList = _context.User.Include(user=>user.RemitRecord)
                                            .Include(user=>user.OftenBeneficiary)
                                            .Include(user => user.UserArc).ThenInclude(userArc => userArc.LastArcScanRecord)
                                               .Include(user => user.UserRegisterType).Where(user=>user.Id == userId).ToList();
            _context.User.RemoveRange(userList);
            _context.SaveChanges();
            return 0;
        }
    }
}
