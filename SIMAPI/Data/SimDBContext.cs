using Microsoft.EntityFrameworkCore;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.Tracking;
using SIMAPI.Data.Models.CommissionStatement;
using SIMAPI.Data.Models.Dashboard;
using SIMAPI.Data.Models.Report;
using SIMAPI.Data.Models.OnField;
using SIMAPI.Data.Models.Sim;
using SIMAPI.Data.Models.Report.InstantReport;
using SIMAPI.Data.Models.OrderListModels;

namespace SIMAPI.Data
{
    public partial class SIMDBContext : DbContext
    {

        public SIMDBContext(DbContextOptions<SIMDBContext> options) : base(options)
        {
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Section 1: Tables

            modelBuilder.Entity<Area>();
            modelBuilder.Entity<AreaLog>();
            modelBuilder.Entity<AreaMap>();
            modelBuilder.Entity<BaseNetwork>();
            modelBuilder.Entity<Network>();
            modelBuilder.Entity<Shop>();
            modelBuilder.Entity<ShopAgreement>();
            modelBuilder.Entity<ShopContact>();
            modelBuilder.Entity<ShopLog>();
            modelBuilder.Entity<ShopVisit>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<UserDocument>();
            modelBuilder.Entity<UserMap>();
            modelBuilder.Entity<UserRole>();
            modelBuilder.Entity<UserTrack>();
            modelBuilder.Entity<Sim>();
            modelBuilder.Entity<SimMap>();
            modelBuilder.Entity<SimActivation>();
            modelBuilder.Entity<SimTopup>();
            modelBuilder.Entity<SimMapChangeLog>();
            modelBuilder.Entity<Supplier>();
            modelBuilder.Entity<SupplierAccount>();
            modelBuilder.Entity<SupplierProduct>();
            



            modelBuilder.Entity<Category>();
            modelBuilder.Entity<SubCategory>();
            modelBuilder.Entity<Product>();
            modelBuilder.Entity<ProductPrice>();
            modelBuilder.Entity<ProductBundle>();
            modelBuilder.Entity<ProductSize>();
            modelBuilder.Entity<ProductColour>();
            modelBuilder.Entity<OrderDeliveryType>();
            modelBuilder.Entity<OrderStatusType>();
            modelBuilder.Entity<OrderPaymentType>();
            modelBuilder.Entity<Order>();
            modelBuilder.Entity<OrderDetail>();
            modelBuilder.Entity<OrderHistory>();
            modelBuilder.Entity<OrderPayment>();
            modelBuilder.Entity<VwOrders>().HasNoKey();
            modelBuilder.Entity<VwOrderHistory>().HasNoKey();
            modelBuilder.Entity<VwOrderPaymentHistory>().HasNoKey();



            #endregion

            #region Section 1: Models

            modelBuilder.Entity<UserRoleOption>().HasNoKey();
            modelBuilder.Entity<AreaVisitedModel>().HasNoKey();
            modelBuilder.Entity<ShopVisitedModel>().HasNoKey();
            modelBuilder.Entity<DailyReportModel>().HasNoKey();
            modelBuilder.Entity<TrackReportModel>().HasNoKey();
            modelBuilder.Entity<UserTrackDataModel>().HasNoKey();
            modelBuilder.Entity<LatLongInfoModel>().HasNoKey();
            modelBuilder.Entity<SimHistoryModel>().HasNoKey();
            modelBuilder.Entity<SimScanModel>().HasNoKey();
            modelBuilder.Entity<AreaReportModel>().HasNoKey();
            modelBuilder.Entity<InstantActivationReportModel>().HasNoKey();
            modelBuilder.Entity<AgentInstantActivationReportModel>().HasNoKey();
            modelBuilder.Entity<ShopInstantActivationReportModel>().HasNoKey();
            modelBuilder.Entity<ShopDetails>().HasNoKey();
            modelBuilder.Entity<UserDetails>().HasNoKey();
            modelBuilder.Entity<ProductDetails>().HasNoKey();
            modelBuilder.Entity<OrderDetailsModel>().HasNoKey();
            modelBuilder.Entity<OrderItemModel>().HasNoKey();

            modelBuilder.Entity<LastDailyActivationReportModel>().HasNoKey();
            modelBuilder.Entity<SalaryReportModel>().HasNoKey();
            modelBuilder.Entity<ShopReportModel>().HasNoKey();
            modelBuilder.Entity<SpamReportModel>().HasNoKey();
            modelBuilder.Entity<OnFieldActivationModel>().HasNoKey();
            modelBuilder.Entity<OnFieldCommissionModel>().HasNoKey();
            modelBuilder.Entity<OnFieldActivationModel>().HasNoKey();
            modelBuilder.Entity<OnFieldGivenVsActivation>().HasNoKey();
            modelBuilder.Entity<ShopVisitHistoryModel>().HasNoKey();
            modelBuilder.Entity<ShopWalletAmountModel>().HasNoKey();
            modelBuilder.Entity<ShopWalletHistoryModel>().HasNoKey();
            modelBuilder.Entity<AreaWiseActivationReportModel>().HasNoKey();
            modelBuilder.Entity<ManagerWiseActivationReportModel>().HasNoKey();
            modelBuilder.Entity<NetworkActivationReportModel>().HasNoKey();
            modelBuilder.Entity<UserWiseActivationReportModel>().HasNoKey();
            modelBuilder.Entity<UserWiseKPIReportModel>().HasNoKey();
            modelBuilder.Entity<CommissionShopListModel>().HasNoKey();
            modelBuilder.Entity<CommissionStatementModel>().HasNoKey();
            modelBuilder.Entity<MonthlyAreaActivationModel>().HasNoKey();
            modelBuilder.Entity<MonthlyShopActivationModel>().HasNoKey();
            modelBuilder.Entity<DashboardMetricsModel>().HasNoKey();
            modelBuilder.Entity<DashboardChartMetricsModel>().HasNoKey();

            modelBuilder.Entity<MonthlyActivationModel>().HasNoKey();
            modelBuilder.Entity<MonthlyHistoryActivationModel>().HasNoKey();
            modelBuilder.Entity<NetworkUsageModel>().HasNoKey();
            modelBuilder.Entity<DailyGivenCountModel>().HasNoKey();
            modelBuilder.Entity<KPITargetReportModel>().HasNoKey();
            modelBuilder.Entity<CommissionListModel>().HasNoKey();
            modelBuilder.Entity<ProductListModel>().HasNoKey();
            modelBuilder.Entity<AllocateAreaDetails>().HasNoKey();
            modelBuilder.Entity<AllocateAgentDetails>().HasNoKey();
            modelBuilder.Entity<OrderListViewModel>().HasNoKey();

            #endregion

            #region relationships


            #endregion
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //        => optionsBuilder.UseSqlServer("Data Source=HW0440;Initial Catalog=SIMDB;Integrated Security=True;TrustServerCertificate=True");



    }
}
