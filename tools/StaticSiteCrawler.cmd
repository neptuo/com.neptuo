StaticSiteCrawler.exe http://localhost:61057/ ..\artifacts\StaticSite / /404.html

xcopy ..\Neptuo.WebSite\Content ..\artifacts\StaticSite\Content /E /R /Y
xcopy ..\Neptuo.WebSite\Scripts ..\artifacts\StaticSite\Scripts /E /R /Y

pause