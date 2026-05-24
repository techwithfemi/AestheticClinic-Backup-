$filePath = 'AestheticEMR\AestheticEMR.Core\Infrastructure\ApplicationDbContext.cs'
$content = Get-Content $filePath -Raw

$searchPattern = "            builder.Entity<vwEmployee>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwEmployees`");
            });
        }"

$replaceWith = "            builder.Entity<vwEmployee>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwEmployees`");
            });

            builder.Entity<VwhRecord>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwhRecords`");
            });

            builder.Entity<VwhRevenueType>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwhRevenueType`");
            });

            builder.Entity<VwhRetainership>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwhRetainership`");
            });

            builder.Entity<VwhretainershipAll>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwhretainershipAll`");
            });

            builder.Entity<VwhRevenueForAcctSale>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwhRevenueForAcctSales`");
            });

            builder.Entity<QryhRecordsUnion>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"qryhRecordsUnion`");
            });

            builder.Entity<VwHreferal>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwHreferal`");
            });

            builder.Entity<VwBillsForClientsBatchVal>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwBillsForClientsBatchVal`");
            });

            builder.Entity<VwhRevenueForAcct>(entity =>
            {
                entity.HasNoKey();
                entity.ToView(`"vwhRevenueForAccts`");
            });
        }"

if ($content.Contains($searchPattern)) {
    $content = $content.Replace($searchPattern, $replaceWith)
    Set-Content $filePath -Value $content
    Write-Host "Configurations added successfully"
} else {
    Write-Host "Pattern not found"
}
