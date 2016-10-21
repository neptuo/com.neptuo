StaticSiteCrawler.exe http://localhost:7287/ ..\artifacts\StaticSite / /404.html

xcopy ..\src\Neptuo.WebSite\Content ..\artifacts\StaticSite\Content /E /R /Y
xcopy ..\src\Neptuo.WebSite\Scripts ..\artifacts\StaticSite\Scripts /E /R /Y

pause