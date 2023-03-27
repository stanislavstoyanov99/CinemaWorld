# ASP.NET Core Cinema World

[![Build Status](https://dev.azure.com/DyNaMiXx7/Cinema%20World/_apis/build/status/stanislavstoyanov99.CinemaWorld?branchName=master)](https://dev.azure.com/DyNaMiXx7/Cinema%20World/_build/latest?definitionId=1&branchName=master)
[![GitHub license](https://img.shields.io/github/license/stanislavstoyanov99/CinemaWorld?color=brightgreen)](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/LICENSE)

## :point_right: Project Introduction :point_left:

**Cinema World** is a ready-to-use ASP.NET Core application.

## :pencil: Project Description EN
The web application provides modern graphical interface for work. The application combines a lot of functionality which can be useful for all types of users. In the navbar like many web applications there is easy to use navigational panel with the following menus: “Home”, “Movies”, “Genres” – with a dropping menu, “News”, “Schedule”. Above this navbar there is a search bar which can find the requested information from the user using searching in the whole system. In the footer of each page there is a reference with links to other pages of the system – “Home”, “News”, “Schedule”, “FAQ”, “Action”, “Adventure”, “Comedy”, “Drama”, “Contact us”. In this section of the page there is also an option for subscription to the system, so in the future you will receive notifications on provided email for new movies, news and updated schedule. Moreover each web page contains three “social” buttons to social networks – Facebook, Twitter, Instagram. Now let’s continue with a brief description for each of the pages.

**_Description of the “Home” page:_** In an interactive slider there is a visualization of six movies from the whole database with IMDB rate over 6. Each of the movies in this slider contains short description of the plot. Below the slider there is another movable slider which visualizes all movies from the database ordered by ascending order of their user rating (there is an embedded rating system for each movie so the user can submit only one vote for 24 hours) and after that by release year. In section “Featured” there are 3 subcategories of movies – “Top watched”, “Top rating” – in this section user can vote, “Recently added”. In the last section of this page there is a another movable slider with top 3 popular movies which have 4 or more stars.

**_Description of the “Movies” page:_** In this page with a tabular view are presented all movies and for convenience there is a visualization of 10 movies per page using paging. Above the table is the current number of movies viewed and is implemented a fast search bar which can find movies by name. For more convenient use above the search bar there is a paging with letters and digits. The most essential information is showed briefly in the table for each movie. When the movie poster is clicked, the user is sent to the page for the given movie, where he can get more information about it.

**_Description of the “Genres” page:_** The page is like a dropping menu, where the user can sort movies by genre. When a genre is chosen, the page renders and shows 12 movies, so for ease of use there is again a paging.

**_Description of the “News” page:_** In the online system there is an integration of a system for news, so the user can receive interesting and various information for movies. Each news shows total count of views, whose the writer is (administrator, moderator, editor) and at what day and time is written. For ease of use there is a visualization of six news. Right to the sidebar are visualized only updated news and a tiny label with caption “new” stays for 12 hours after the update. In this sidebar are also the most viewed top news.

**_Description of the “Schedule” page:_** In this page you can find a schedule of movies and again there is a paging which shows only five projections per page. For convenience you can filter projections by cinemas. Each projection contains in detail a description of the movie like director, rating as well as a button for booking a seat in the hall. The user can choose a seat in the hall and request what type of ticket wants. Each ticket initially costs 10$. In the right sidebar there is a section with movie reviews, which is under development.

**_Description of “FAQ” page:_** Here you can find information for the most frequent asked questions.

**_Description of “Contact us” page:_** Here you can send your inquery, get information about mobile phones and emails that you can write and again links to social networks.

**_Description of administrator panel:_** Like other systems here there is an administrator panel where the admin can add, delete, edit information about the system. In section “User’s Administration” for ease of use the administrator can send directly emails to users who already sent their inqueries.

**_Description of the user profile:_** For the user profile there is a standard functionality which is provided by ASP.NET Core Identity.

**_Additional functionalities:_** There is an integration of fast pop-up form for login/registration. There is also Facebook login which can be used instead of standard registration. Each movie news page contains comments and subcomments.

**_In conclusion:_** Cinema World is a project which combines in one place convenient user interface, chance to look for movies, news for them and reservation of tickets in real time. Furthermore, there is an integrated rating system which is an additional user experience. In future there is a plan for developing a real system for ticket payment and form for movie reviews. The purpose of the system is to be similar to IMDB and in addition also to provide the opportunity to purchase tickets.

## :pencil: Project Description BG
Уеб приложението предоставя модерен графичен интерфейс за работа. Приложението комбинира в себе си много функционалност, която може да бъде полезна за потребителите. В горната му част като повечето уеб приложения има удобен навигационен панел със следните менюта: “Home”, “Movies”, “Genres” – с падащо меню, “News”, “Schedule”. Над този панел има търсачка, която може да намира заявена информация от потребителя като резултатите, които се връщат са след претърсване на цялата система. В дъното на всяка страница има footer или мястото, където може да намерите препратка към останалите страници от системата – “Home”, “News”, “Schedule”, “FAQ”, “Action”, “Adventure”, “Comedy”, “Drama”, “Contact us”. В тази част от страницата също има опция за абониране (subscribe) към системата, т.е в бъдеще може да получавате известия за нови филми, новини и обновени разписания. Също така във всяка една страница от системата има три “социални” бутона към социалните мрежи – Facebook, Twitter, Instagram. Нека започнем с описание на всяка една от страниците.

**_Описание на начална страница “Home”:_**
В интерактивен слайдър се визуализират 6 филма от цялата база данни с IMDB рейтинг по-голям от 6. Всеки един филм в този слайдър съдържа кратко описание на сюжета. Под слайдъра се намира друг подвижен слайдър, където се визуализират всички филми от базата, подредени по възходящ ред на потребителския им рейтинг (има вградена рейтинг система за всеки един филм, като потребителят има право само на 1 глас в рамките на 24 часа) и след това по година на излизане. В секция “Featured” има 3 подкатегории на филми – “Top watched”, “Top rating”, “Recently added”, съотвено филми, които имат най-много преглеждания, филми с най-висок потребителски рейтинг – в тази секция потребителят може да гласува и последно добавени филми. В последната част от тази страница се намира подвижен слайдър с 3 най-известни филма, които имат 4 или повече звезди.

**_Описание на страница “Movies”:_**
В тази страница в табличен вид са представени всички филми, като за удобство се визуализират по 10 филма на страница чрез използване на странициране. Над таблицата е изписан текущият брой визуализирани филми и е имплементирана бърза търсачка по име на филм. За още по-голямо удобство над тази търсачка има странициране по букви и цифри. В таблицата за всеки един филм е визуализирана най-важната информация. При кликване на постера, потребителят бива изпратен към страницата за дадения филм, където може да получи повече информация за него.

**_Описание на страница “Genres”:_**
Самата страница представлява падащо меню, където потребителят може да сортира филмите по даден жанр. При избор на даден жанр се визуализират по 12 филма, като за удобство отново се ползва странициране.

**_Описание на страница “News”:_**
В онлайн системата е интегрирана и система за новини, т.е потребителят може да получи интересна и разнообразна информация за филмите. Всяка новина съхранява в себе си броя преглеждания, от кой потребител (админ, модератор, редактор) е написана и в какъв ден и час. За удобството се визуализират 6 новини. Отдясно в sidebar-а се визуализират само обновените новини, като излиза етикет “new” в продължение на 12 часа след обновяването. В този sidebar се визуализират и топ новините, т.е тези с най-много преглеждания.

**_Описание на страница “Schedule”:_**
В тази страница може да намерите разписание на филмите като отново е ползвано странициране и виждате по 5 прожекции на страница. За удобство може да филтрирате прожекциите по кина. Всяка една прожекция има подробно описание за филма като режисьор, рейтинг и т.н, както и бутон за резервация на място в залата. При резервацията потребителят може да избере място в залата и да заяви какъв тип билет иска, като първоначално всеки билет струва 10$. В sidebar-а отдясно се намира секция с ревюта за филми, която е в процес на изработка.

**_Описание на страница “FAQ”:_**
Тук можете да намирате информация за най-често задаваните въпроси.

**_Описание на страница “Contact us”:_**
Тук можете да изпратите вашето запитване, да получите информация за телефонните номера, имейли, на които можете да пишете при запитвания и отново връзки към социалните мрежи.
Описание на админ панела:
Като всяка една система и тази има админ панел, където може да бъде добавяна, изтривана и редактирана информация за системата. В секция “User’s Administration” за удобство админът може да изпраща директно имейли към потребителите, които вече са изпратили запитване през контактната форма.

**_Описание на потребителския профил:_**
За потребителския профил е използвана стандартната функционалност, която се предоставя от ASP.NET Core Identity.

**_Допълнителни функционалности:_**
Интегриран е бърза изскачаща форма за логин и регистрация. На лице също така е и Facebook login, който може да бъде използван вместо стандартната регистрация. Във всяка една страница за новина може да се постват коментари и подкоментари.

**_Заключение:_**
Cinema World е проект, което обединява на едно място удобен потребителски интерфейс, възможност за преглед на филми, новини за тях и резервация на билети по избрано кино. Също така е интегрирана рейтинг система, която е допълнително потребителско изживяване. В бъдеще се планира изграждането на реална система за плащане на билети както и създаване на ревюта за филми. Целта е системата да бъде подобна на IMDB, като в допълнение предоставя възможност и за онлайн закупуване на билети.

## Unit tests Code coverage

![Code coverage](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/tests-code-coverage.png)

## :hammer: Used technologies
* ASP.NET [CORE 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1 "CORE 3.1") MVC
* ASP.NET Core areas
* Entity Framework [CORE 3.1](https://docs.microsoft.com/en-us/ef/core/ "CORE 3.1")
* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/ "Newtonsoft.Json")
* [SendGrid](https://github.com/sendgrid)
* [SendGrid Widget](https://sgwidget.com/ "SendGrid Widget")
* [Cloudinary](https://github.com/cloudinary/CloudinaryDotNet)
* [HtmlSanitizer](https://github.com/mganss/HtmlSanitizer)
* [TinyMCE](https://github.com/tinymce/)
* [Bootstrap](https://github.com/twbs/bootstrap)
* [Moment.js](https://www.nuget.org/packages/Moment.js/ "Moment.js")
* AJAX real-time Requests
* [jQuery](https://github.com/jquery/jquery) and any kind of jQuery plugins ([bootstrap-select](https://developer.snapappointments.com/bootstrap-select/ "bootstrap-select"))
* JavaScript and JS animations
* [Facebook for developers](https://developers.facebook.com)
* [xUnit](https://github.com/xunit/xunit)
* In-Memmory Cache

## :floppy_disk: Database Diagram
![](https://res.cloudinary.com/cinemaworld/image/upload/v1589836846/dbDiagram_vo8k3k.jpg)

## Link
https://cinemaworld.azurewebsites.net

## Screenshots

### Home page
![Home page 1](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/home-page-1.jpg)
![Home page 2](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/home-page-2.jpg)
![Home page 3](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/home-page-3.jpg)

### Login/Register Dialog
![Login Register Dialog 1](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/login-register-dialog-1.jpg)
![Login Register Dialog 2](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/login-register-dialog-2.jpg)

### Footer
![Footer](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/footer.jpg)

### Movies page
![Movies page](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/movies-page.jpg)
![Movies single page](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/movies-page-single.jpg)
![Movies add comment](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/movies-add-comment.jpg)
![Movies sub comments](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/movies-sub-comments.jpg)

### Genres page
![Genres page](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/genres-page.jpg)

### News page
![News page 1](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/news-page-1.jpg)
![News page 2](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/news-page-2.jpg)
![News single page](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/news-single-page.jpg)

### FAQ page
![FAQ page](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/faq-page.jpg)

### Privacy page
![Privacy page](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/privacy-page.jpg)

### Schedule page
![Schedule page](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/schedule-page.jpg)

### Book ticket page
![Book ticket page 1](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/book-ticket-page-1.jpg)
![Book ticket page 2](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/book-ticket-page-2.jpg)
![Book ticket page 2](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/book-ticket-page-3.jpg)

### Contacts page
![Contacts page 1](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/contacts-page-1.jpg)
![Contacts page 2](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/contacts-page-2.jpg)

### Admin Dashboard
![Admin Dashboard](https://github.com/stanislavstoyanov99/CinemaWorld/blob/master/screenshots/admin-dashboard.jpg)

## Author

[Stanislav Stoyanov](https://github.com/stanislavstoyanov99)
- Facebook: [@Станислав Стоянов](https://www.facebook.com/profile.php?id=100000714808058)
- LinkedIn: [@stanislavstoyanov99](https://www.linkedin.com/in/stanislavstoyanov99/)

## Template authors

- [Nikolay Kostov](https://github.com/NikolayIT)
- [Vladislav Karamfilov](https://github.com/vladislav-karamfilov)

## :v: Show your opinion

Give a :star: if you like this project!

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details
