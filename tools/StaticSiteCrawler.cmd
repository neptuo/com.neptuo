StaticSiteCrawler.exe http://localhost:7287/ ..\artifacts\StaticSite / /404.html

xcopy src\Neptuo.WebSite\Content\Images ..\artifacts\StaticSite\Content\Images /E /R /Y
xcopy src\Neptuo.WebSite\Content\bootstrap.min.css ..\artifacts\StaticSite\Content /R /Y
xcopy src\Neptuo.WebSite\Content\bootstrap-theme.min.css ..\artifacts\StaticSite\Content /R /Y
xcopy src\Neptuo.WebSite\Content\web.css ..\artifacts\StaticSite\Content /R /Y
xcopy src\Neptuo.WebSite\fonts ..\artifacts\StaticSite\fonts /E /R /Y
xcopy src\Neptuo.WebSite\Scripts ..\artifacts\StaticSite\Scripts /E /R /Y

pause