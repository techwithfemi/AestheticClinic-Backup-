#!/bin/bash
# Create a file named deploy-local.sh in your project root and add this:
# 1. Navigate to client and build Angular with the local subfolder path
cd AestheticEMR/AestheticEMR.client
npm ci
npx ng build --configuration production --base-href /emr/
cd ../..

# 2. Build and publish the .NET Backend locally
dotnet publish AestheticEMR/AestheticEMR.Server/AestheticEMR.Server.csproj --configuration Release --output ./publish

# 3. Copy the compiled publish files straight into your local IIS folder
# (Using cp -r to clear/overwrite the folder)
rm -rf /c/inetpub/wwwroot/emr/*
cp -r ./publish/* /c/inetpub/wwwroot/emr/

# 4. Refresh local IIS
iisreset

echo "🚀 Local deployment to http://localhost/emr complete!"