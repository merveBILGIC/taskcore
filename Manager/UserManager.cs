using System;
using taskcore.Dao;
using taskcore.Models;
using System.Linq;

namespace taskcore.Manager
{
    public class UserManager
    {

        public static User user = null;
        public UserDao userDao = null;

        public static User GetCurrentUser()
        {
            return user;
        }

        public static void SetCurrentUser(User usr)
        {
            user = usr;
        }

        public UserDao GetUserDao(){
            return userDao == null ? userDao = UserDao.getInstance() : userDao;
        }



    }
}