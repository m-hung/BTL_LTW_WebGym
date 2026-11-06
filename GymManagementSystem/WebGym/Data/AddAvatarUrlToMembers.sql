-- =====================================================================
-- SCRIPT BỔ SUNG CỘT HÌNH ẢNH (AvatarUrl) CHO BẢNG MEMBERS
-- Dự án: WebGym Management System
-- =====================================================================

USE [GymManagementSystemDb];
GO

-- 1. Thêm cột AvatarUrl vào bảng Members nếu chưa có
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[Members]') 
      AND name = N'AvatarUrl'
)
BEGIN
    ALTER TABLE [dbo].[Members]
    ADD [AvatarUrl] NVARCHAR(500) NULL;
    
    PRINT N'[THÀNH CÔNG] Đã thêm cột AvatarUrl NVARCHAR(500) vào bảng Members!';
END
ELSE
BEGIN
    PRINT N'[THÔNG BÁO] Cột AvatarUrl đã tồn tại trong bảng Members.';
END
GO

-- 2. Đảm bảo gói tập của Member 1 là Gói Vàng khi đã nâng cấp
UPDATE [dbo].[Subscriptions]
SET [PackageId] = 3,
    [Status] = 'Active'
WHERE [MemberId] = 1 
  AND ([PaidAmount] >= 4500000 OR [OriginalPrice] >= 4500000 OR [EndDate] > '2027-01-01');

UPDATE [dbo].[Payments]
SET [PackageId] = 3,
    [Status] = 'Success'
WHERE [MemberId] = 1 
  AND [Amount] >= 4500000;
GO

PRINT N'[HOÀN TẤT] Đồng bộ dữ liệu thành công!';
GO
