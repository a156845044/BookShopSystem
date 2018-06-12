using BookShopSystem.DataAccess.Context;
using BookShopSystem.DataAccess.Entity;
using BookShopSystem.Models;
using BookShopSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopSystem.Service
{
    /// <summary>
    /// 图书
    /// </summary>
    public class BookService
    {
        /// <summary>
        /// 获取图书分页列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="recordCount">总记录条数</param>
        /// <returns>图书分页列表</returns>
        public List<BookInfoEntity> GetBookList(string key, int pageIndex, int pageSize, out int recordCount)
        {
            List<BookInfoEntity> list = new List<BookInfoEntity>();
            StringBuilder strComom = new StringBuilder();
            strComom.Append(" FROM [dbo].[Product] P ");
            strComom.AppendFormat(" LEFT JOIN [dbo].[FileUpload] FU ON FU.[SourceId]=P.Id and [SourceType]={0} and [IsDefault]=1 ", FlagMgr.Upload.SourceType.Book.ToInt());
            strComom.Append(" INNER JOIN [dbo].[Category] child on child.[Id]=P.[CategoryId] ");
            strComom.Append(" INNER JOIN [dbo].[Category] parent on parent.[Id]=child.[ParentId] ");
            List<SqlParameter> paramList = new List<SqlParameter>();
            bool flag = false;
            if (!string.IsNullOrEmpty(key))
            {
                strComom.Append(" WHERE  P.[Title] LIKE (@Key) or P.[Author] LIKE (@Key) or P.[ISBN] LIKE (@Key) or child.[CategoryName]  LIKE (@Key) or parent.[CategoryName]  LIKE (@Key)");
                paramList.Add(new SqlParameter("@Key", SqlDbType.VarChar) { Value = string.Format("%{0}%", key) });
                flag = true;
            }
            recordCount = 0;
            string coutsql = string.Format("select count(P.Id) {0}", strComom.ToString());
            string selectsql = string.Format("SELECT P.*,FU.[FilePath] AS CoverURL,child.[CategoryName] AS CategoryName,parent.CategoryName AS ParentCategoryName,parent.Id AS ParentCategoryId  {0}", strComom.ToString()).PageLimit(pageIndex, pageSize, " P.Id desc ");
            using (var ctx = new BookShopContext())
            {

                if (flag)
                {
                    recordCount = ctx.Database.SqlQuery<int>(coutsql, paramList.ToArray()).FirstOrDefault();
                    list = ctx.Database.SqlQuery<BookInfoEntity>(selectsql, paramList.ToCopyArray()).ToList();
                }
                else
                {
                    recordCount = ctx.Database.SqlQuery<int>(coutsql).FirstOrDefault();
                    list = ctx.Database.SqlQuery<BookInfoEntity>(selectsql).ToList();
                }
            }
            return list;
        }

        /// <summary>
        /// 添加图书
        /// </summary>
        /// <param name="book">待添加的实体</param>
        /// <param name="imgList">待添加的图片</param>
        /// <returns>执行结果</returns>
        public bool AddBook(Product book, List<FileUpload> imgList)
        {
            bool flag = false;
            int sourceType = FlagMgr.Upload.SourceType.Book.ToInt();
            using (var cxt = new BookShopContext())
            {
                using (DbContextTransaction transaction = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        cxt.Product.Add(book);
                        flag = cxt.SaveChanges() > 0;
                        if (imgList.Count > 0)
                        {
                            for (int i = 0; i < imgList.Count; i++)
                            {
                                if (i == 0)
                                {
                                    imgList[i].IsDefault = true;
                                }
                                else
                                {
                                    imgList[i].IsDefault = false;
                                }
                                imgList[i].SourceType = sourceType;
                                imgList[i].SourceId = book.Id.ToString();
                            }
                            cxt.FileUpload.AddRange(imgList);
                            cxt.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error occurred.");
                    }
                }


            }
            return flag;
        }

        /// <summary>
        /// 获取图书信息
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns>图书信息</returns>
        public BookInfoEntity GetBookInfo(long productId)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" SELECT P.*,FU.[FilePath] AS CoverURL,child.[CategoryName] AS CategoryName,parent.CategoryName AS ParentCategoryName,parent.Id AS ParentCategoryId ");
            strSQL.Append(" FROM [dbo].[Product] P ");
            strSQL.AppendFormat(" LEFT JOIN [dbo].[FileUpload] FU ON FU.[SourceId]=P.Id and [SourceType]={0} and [IsDefault]=1 ", FlagMgr.Upload.SourceType.Book.ToInt());
            strSQL.Append(" INNER JOIN [dbo].[Category] child on child.[Id]=P.[CategoryId] ");
            strSQL.Append(" INNER JOIN [dbo].[Category] parent on parent.[Id]=child.[ParentId] ");
            strSQL.Append(" WHERE  P.[Id] =@Id");

            var param = new SqlParameter[] {
                new SqlParameter("@Id", SqlDbType.BigInt) { Value = productId }
                };

            using (var ctx = new BookShopContext())
            {
                return ctx.Database.SqlQuery<BookInfoEntity>(strSQL.ToString(), param).FirstOrDefault();
            }

        }

        /// <summary>
        /// 更新图书
        /// </summary>
        /// <param name="book">待添加的实体</param>
        /// <param name="imgList">待添加的图片</param>
        /// <returns>执行结果</returns>
        public bool UpdateBook(Product book, List<FileUpload> imgList)
        {
            bool flag = false;
            string bookId = book.Id.ToString();
            int type = FlagMgr.Upload.SourceType.Book.ToInt();
            int sourceType = FlagMgr.Upload.SourceType.Book.ToInt();
            using (var cxt = new BookShopContext())
            {
                using (DbContextTransaction transaction = cxt.Database.BeginTransaction())
                {
                    try
                    {
                        cxt.Entry<Product>(book).State = EntityState.Modified;
                        flag = cxt.SaveChanges() > 0;

                        var tempImg = cxt.FileUpload.Where(e => e.SourceId == bookId && e.SourceType == type).ToList();
                        if (tempImg.Count > 0)
                        {
                            cxt.FileUpload.RemoveRange(tempImg);
                            cxt.SaveChanges();
                        }

                        if (imgList.Count > 0)
                        {
                            for (int i = 0; i < imgList.Count; i++)
                            {
                                if (i == 0)
                                {
                                    imgList[i].IsDefault = true;
                                }
                                else
                                {
                                    imgList[i].IsDefault = false;
                                }
                                imgList[i].SourceType = sourceType;
                                imgList[i].SourceId = book.Id.ToString();
                            }
                            cxt.FileUpload.AddRange(imgList);
                            cxt.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error occurred.");
                    }
                }


            }
            return flag;
        }

        /// <summary>
        /// 删除图书
        /// </summary>
        /// <param name="productId">图书编号</param>
        /// <returns>执行结果</returns>
        public bool DeleteBook(long productId)
        {
            int type = FlagMgr.Upload.SourceType.Book.ToInt();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("DELETE FROM [dbo].[FileUpload] WHERE [SourceType]={0} and [SourceId]=@SourceId ;", type);
            strSQL.Append("DELETE FROM [dbo].[Product] WHERE [Id]=@Id ;");
            var param = new SqlParameter[] {
                new SqlParameter("@SourceId", SqlDbType.VarChar) { Value = productId.ToString() },
                new SqlParameter("@Id", SqlDbType.BigInt) { Value = productId }
                };
            using (var ctx = new BookShopContext())
            {
                return ctx.Database.ExecuteSqlCommand(strSQL.ToString(), param) > 0;
            }
        }

        /// <summary>
        /// 获取好书推荐列表
        /// </summary>
        /// <param name="topN">显示条数</param>
        /// <param name="professionId">职业编号</param>
        /// <returns>推荐图书列表</returns>
        public List<BookInfoEntity> GetTopNList(long professionId = 0, int topN = 3, long productId = 0)
        {
            List<BookInfoEntity> list = new List<BookInfoEntity>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("SELECT top {0} P.*,FU.[FilePath] AS CoverURL,child.[CategoryName] AS CategoryName,parent.CategoryName AS ParentCategoryName,parent.Id AS ParentCategoryId FROM [dbo].[Product] P ", topN);
            strSQL.AppendFormat(" LEFT JOIN [dbo].[FileUpload] FU ON FU.[SourceId]=P.Id and [SourceType]={0} and [IsDefault]=1 ", FlagMgr.Upload.SourceType.Book.ToInt());
            strSQL.Append(" INNER JOIN [dbo].[Category] child on child.[Id]=P.[CategoryId] ");
            strSQL.Append(" INNER JOIN [dbo].[Category] parent on parent.[Id]=child.[ParentId] WHERE 1=1 ");
            if (professionId > 0)
            {
                strSQL.AppendFormat(" and  P.[ProfessionId]={0}", professionId);

            }
            if (productId > 0)
            {
                strSQL.AppendFormat(" and  P.[Id]!={0}", productId);
            }
            strSQL.Append(" ORDER BY p.ClickCount asc ");
            using (var ctx = new BookShopContext())
            {
                list = ctx.Database.SqlQuery<BookInfoEntity>(strSQL.ToString()).ToList();
            }
            return list;
        }

        /// <summary>
        /// 根据分类获取图书列表
        /// </summary>
        /// <param name="categoryId">分类列表</param>
        /// <param name="topN">显示条数</param>
        /// <returns>推荐图书列表</returns>
        public List<BookInfoEntity> GetTopNListByCategory(long categoryId, int topN = 3)
        {
            List<BookInfoEntity> list = new List<BookInfoEntity>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.AppendFormat("SELECT top {0} P.*,FU.[FilePath] AS CoverURL,child.[CategoryName] AS CategoryName,parent.CategoryName AS ParentCategoryName,parent.Id AS ParentCategoryId FROM [dbo].[Product] P ", topN);
            strSQL.AppendFormat(" LEFT JOIN [dbo].[FileUpload] FU ON FU.[SourceId]=P.Id and [SourceType]={0} and [IsDefault]=1 ", FlagMgr.Upload.SourceType.Book.ToInt());
            strSQL.Append(" INNER JOIN [dbo].[Category] child on child.[Id]=P.[CategoryId] ");
            strSQL.Append(" INNER JOIN [dbo].[Category] parent on parent.[Id]=child.[ParentId] ");

            strSQL.AppendFormat(" WHERE  P.[CategoryId] IN(SELECT [Id] from [dbo].[Category] where [ParentId]={0})", categoryId);

            strSQL.Append(" ORDER BY p.ClickCount asc ");
            using (var ctx = new BookShopContext())
            {
                list = ctx.Database.SqlQuery<BookInfoEntity>(strSQL.ToString()).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取销量统计
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns>销量统计</returns>
        public int GetSaleCount(long productId)
        {
            string sql = string.Format("select ISNULL( SUM(OD.[Quantity]),0) from [dbo].[Orders] O INNER JOIN [dbo].[OrdersDetail] OD ON OD.[OrderId]=O.[Id] WHERE OD.[ProductId]={0}", productId);
            int sales = 0;
            using (var ctx = new BookShopContext())
            {
                sales = ctx.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
            return sales;
        }

        /// <summary>
        /// 获取当月销量统计
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns>销量统计</returns>
        public int GetSaleCountByMonth(long productId)
        {
            string sql = string.Format("select ISNULL( SUM(OD.[Quantity]),0) from [dbo].[Orders] O INNER JOIN [dbo].[OrdersDetail] OD ON OD.[OrderId]=O.[Id] WHERE OD.[ProductId]={0} and datediff(month,O.[CreateTime],getdate())=0", productId);
            int sales = 0;
            using (var ctx = new BookShopContext())
            {
                sales = ctx.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
            return sales;
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns></returns>
        public int GetCommentCount(long productId)
        {
            string sql = string.Format("select count(*) from [dbo].[ProductComment] where [ProductId]={0} and [StateFlag]=1", productId);
            int count = 0;
            using (var ctx = new BookShopContext())
            {
                count = ctx.Database.SqlQuery<int>(sql).FirstOrDefault();
            }
            return count;
        }

        /// <summary>
        /// 获取商品详情
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns>图书信息</returns>
        public ProductInfoEntity GetBookDetailInfo(long productId)
        {
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append(" SELECT P.*,child.[CategoryName] AS CategoryName,parent.CategoryName AS ParentCategoryName,parent.Id AS ParentCategoryId,publishers.[Name] AS PublisherName ");
            strSQL.Append(" FROM [dbo].[Product] P ");
            strSQL.Append(" INNER JOIN [dbo].[Category] child on child.[Id]=P.[CategoryId] ");
            strSQL.Append(" INNER JOIN [dbo].[Category] parent on parent.[Id]=child.[ParentId] ");
            strSQL.Append(" INNER JOIN [dbo].[Publishers] publishers on publishers.[Id]=P.[PublisherId] ");
            strSQL.Append(" WHERE  P.[Id] =@Id");
            var param = new SqlParameter[] {
                new SqlParameter("@Id", SqlDbType.BigInt) { Value = productId }
            };
            using (var ctx = new BookShopContext())
            {
                return ctx.Database.SqlQuery<ProductInfoEntity>(strSQL.ToString(), param).FirstOrDefault();
            }
        }

        /// <summary>
        /// 更新点击量
        /// </summary>
        /// <param name="productId">商品编号</param>
        /// <returns></returns>
        public bool UpdateClick(long productId)
        {
            string sql = string.Format("UPDATE [dbo].[Product] SET [ClickCount]=[ClickCount]+1 WHERE [Id]={0}", productId);
            using (var ctx = new BookShopContext())
            {
                return ctx.Database.ExecuteSqlCommand(sql) > 0;
            }
        }


        /// <summary>
        ///根据商品编号获取商品信息列表
        /// </summary>
        /// <param name="productIdStrList">商品编号列表</param>
        /// <returns>推荐图书列表</returns>
        public List<BookPayInfoEntity> GetPayInfoList(string productIdStrList)
        {
            List<BookPayInfoEntity> list = new List<BookPayInfoEntity>();
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SELECT P.*,FU.[FilePath] AS CoverURL FROM [dbo].[Product] P ");
            strSQL.AppendFormat(" LEFT JOIN [dbo].[FileUpload] FU ON FU.[SourceId]=P.Id and [SourceType]={0} and [IsDefault]=1 ", FlagMgr.Upload.SourceType.Book.ToInt());
            strSQL.AppendFormat(" WHERE  P.Id IN({0}) ", productIdStrList);
            using (var ctx = new BookShopContext())
            {
                list = ctx.Database.SqlQuery<BookPayInfoEntity>(strSQL.ToString()).ToList();
            }
            return list;
        }

        /// <summary>
        /// 获取图书分页列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="recordCount">总记录条数</param>
        /// <returns>图书分页列表</returns>
        public List<BookInfoEntity> GetBookList(long categoryId, string key, int pageIndex, int pageSize, out int recordCount)
        {
            List<BookInfoEntity> list = new List<BookInfoEntity>();
            StringBuilder strComom = new StringBuilder();
            strComom.Append(" FROM [dbo].[Product] P ");
            strComom.AppendFormat(" LEFT JOIN [dbo].[FileUpload] FU ON FU.[SourceId]=P.Id and [SourceType]={0} and [IsDefault]=1 ", FlagMgr.Upload.SourceType.Book.ToInt());
            strComom.Append(" INNER JOIN [dbo].[Category] child on child.[Id]=P.[CategoryId] ");
            strComom.Append(" INNER JOIN [dbo].[Category] parent on parent.[Id]=child.[ParentId] WHERE 1=1");
            List<SqlParameter> paramList = new List<SqlParameter>();
            bool flag = false;
            if (categoryId > 0)
            {
                strComom.AppendFormat(" and P.CategoryId={0} ", categoryId);
            }
            if (!string.IsNullOrEmpty(key))
            {
                strComom.Append(" AND (  P.[Title] LIKE (@Key) or P.[Author] LIKE (@Key) or P.[ISBN] LIKE (@Key) or child.[CategoryName]  LIKE (@Key) or parent.[CategoryName]  LIKE (@Key) ) ");
                paramList.Add(new SqlParameter("@Key", SqlDbType.VarChar) { Value = string.Format("%{0}%", key) });
                flag = true;
            }
            recordCount = 0;
            string coutsql = string.Format("select count(P.Id) {0}", strComom.ToString());
            string selectsql = string.Format("SELECT P.*,FU.[FilePath] AS CoverURL,child.[CategoryName] AS CategoryName,parent.CategoryName AS ParentCategoryName,parent.Id AS ParentCategoryId  {0}", strComom.ToString()).PageLimit(pageIndex, pageSize, " P.Id desc ");
            using (var ctx = new BookShopContext())
            {
                if (flag)
                {
                    recordCount = ctx.Database.SqlQuery<int>(coutsql, paramList.ToArray()).FirstOrDefault();
                    list = ctx.Database.SqlQuery<BookInfoEntity>(selectsql, paramList.ToCopyArray()).ToList();
                }
                else
                {
                    recordCount = ctx.Database.SqlQuery<int>(coutsql).FirstOrDefault();
                    list = ctx.Database.SqlQuery<BookInfoEntity>(selectsql).ToList();
                }
            }
            return list;
        }
    }
}
