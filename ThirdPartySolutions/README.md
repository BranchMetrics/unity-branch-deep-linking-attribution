# Branch Metrics Unity
## Third Party Solutions

That folder contain solutions for "conflicts" between Branch SDK and other plugins.

### IOS
1. Unpack sources
2. Replace existed files

*Note: don't forget add your branch key into file named "BranchiOSWrapper.mm"*

### Android
1. Add library to your project (select what library you need)
2. Change android:name to "io.branch.anroidthirdparty.BranchCustomApplication" in tag application
3. Change android:name to "io.branch.anroidthirdparty.BranchCustomActivity" in tag activity

