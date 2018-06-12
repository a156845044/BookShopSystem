using BookShopSystem.DataAccess.Context;
using BookShopSystem.DataAccess.Entity;
using BookShopSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Service
{
    /// <summary>
    /// 分类
    /// </summary>
    public class CategoryService
    {
        /// <summary>
        /// 根据父类编号获取分类列表
        /// </summary>
        /// <param name="parentId">父类编号</param>
        /// <returns>分类列表</returns>
        public List<Category> GetCategoryList(long parentId)
        {
            using (var ctx = new BookShopContext())
            {
                var sql = from c in ctx.Category where c.ParentId == parentId select c;
                return sql.ToList();
            }
        }

        /// <summary>
        /// 获取全部分类列表
        /// </summary>
        /// <returns>分类列表</returns>
        public List<Category> GetCategoryList()
        {
            using (var ctx = new BookShopContext())
            {
                return ctx.Category.ToList();
            }
        }

        /// <summary>
        /// 获取商城首页分类列表
        /// </summary>
        /// <returns>分类列表</returns>
        public List<CategoryEntity> GetBookCategoryList()
        {
            var allList = GetCategoryList();//获取全部分类
            var parentList = allList.FindAll(e => e.ParentId == 0).ToList();
            List<CategoryEntity> list = new List<CategoryEntity>();
            foreach (var item in parentList)
            {
                CategoryEntity parent = new CategoryEntity {
                    Id = item.Id,
                    Name=item.CategoryName
                };
                parent.ChildList = new List<ClildCategoryEntity>();
                var childList =allList.FindAll(e => e.ParentId == item.Id).ToList();
                foreach (var childItem in childList)
                {
                    ClildCategoryEntity child = new ClildCategoryEntity {Id=childItem.Id,Name=childItem.CategoryName };
                    parent.ChildList.Add(child);
                }
                list.Add(parent);
            }
            return list;
        }
    }
}
