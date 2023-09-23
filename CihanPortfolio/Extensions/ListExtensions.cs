using CihanPortfolio.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CihanPortfolio.Extensions
{
    public static class ListExtensions
    {
        
        public static void AddDuplicate<T>(this List<T> list, T item) where T : class
        {
            list.Add(item);
            list.Add(item);
        }
    }
}