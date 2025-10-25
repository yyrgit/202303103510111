#  SEO Optimizer Tool


### Core Features:
* **On-Page Audit:** Checks for the presence and length of the `<title>` tag, `<meta name="description">`, and `<h1>` heading.
* **Keyword Density Checker:** Calculates the frequency and density (%) of 1-word, 2-word, and 3-word phrases on the page.
* **Stop Word Filter:** Allows users to exclude common "stop words" (e.g., 'a', 'the', 'is') for more accurate keyword analysis.
* **Broken Link Check:** Scans the page for internal links and reports any links returning an HTTP 404 error.
* **Analysis History (Intermediate Feature):** Authenticated users can save past reports for comparison.

### Technologies Used:
* **Backend:** ASP.NET [Core/Framework] MVC
* **Language:** C#
* **HTML Parsing:** HTML Agility Pack (HAP) for fetching and parsing the external URL content.
* **Database:** [E.g., SQL Server]
* **Data Access:** Entity Framework Core
* **Frontend:** HTML, CSS, Bootstrap, and **Chart.js** (for visualizing keyword data).

## ⚙️ Installation Steps

These instructions assume you have the **.NET SDK** (version [e.g., 7.0 or 8.0]) and **Visual Studio** installed.

1.  **Clone the Repository:**
    ```bash
    git clone [Your Repository URL Here]
    cd SeoOptimizerTool
    ```

2.  **Configure Database:**
    * Open `appsettings.json` and update the `DefaultConnection` string with your local SQL Server details.
    * Apply migrations to create the database schema:
        ```bash
        dotnet ef database update
        ```

3.  **Restore Dependencies:**
    ```bash
    dotnet restore
    ```

##  How to Run the Project

### Option 1: Using Visual Studio (Recommended)

1.  Open the **`SeoOptimizerTool.sln`** file in Visual Studio.
2.  Set the main project as the startup project.
3.  Press **F5** or click the **'Run'** button.

### Option 2: Using the Command Line (CLI)

1.  Navigate to the project directory containing the `.csproj` file:
    ```bash
    cd [Your Project Folder Name] 
    ```
2.  Run the application:
    ```bash
    dotnet run
    ```
3.  Open the URL displayed in the console (e.g., `http://localhost:5000`) in your web browser.
