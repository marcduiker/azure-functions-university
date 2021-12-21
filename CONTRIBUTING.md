# How to contribute

👋 Hi! Great that you want to contribute to Azure Functions University! 😃

We're currently focussed on creating beginner content for Azure Functions written in C#, TypeScript, Python, and PowerShell, and using VSCode as the code editor.

## Step 1 - GitHub Issues

🔎 Please check if there's an [existing issue](https://github.com/marcduiker/azure-functions-university/issues) which matches your idea. Perhaps you can collaborate with someone on this.

💡 If you have  new idea, please [create a new __Content Request__ issue](https://github.com/marcduiker/azure-functions-university/issues/new?assignees=&labels=content+%F0%9F%93%9D&template=CONTENT-REQUEST.yml&title=Content+Request%3A+%3CTITLE%3E) first where you can describe the topic.

## Step 2 - Process

Once we've discussed the Content Request issue and agree to include the lesson, you can start by forking the [Azure Functions University repository](https://github.com/marcduiker/azure-functions-university) and create a new branch for the topic and language you want to add (e.g. `cosmosdb-dotnet6`).

Try to create a pull request (PR) early in the process of creating the lesson. This way we can see the progress and help where needed. It will probably take a few iterations to get everything right, don't feel discouraged by this process ♥. The quality & consistency of the lesson is more important than the speed of delivering it.

### Lesson Structure

If you are contributing to create a lesson, please take into account the length and the tone of the existing lessons. We want to have a uniform experience across all of our lessons. The latest lessons you can use as a reference are the .NET 6 (dotnet6) ones.

An Azure Functions University lesson consists of several parts:

- A lesson markdown file named `README.md` in the `lessons/{language}/{topic}` folder. Example: _lessons/dotnet6/http/README.md_.
  - Use the [lesson template file](lessons/_lesson_template.md).
  - Give the lesson a short but descriptive title.
  - Describe the goal of the lesson.
  - Break up the lesson in small exercises.
  - Each exercise has a clear sub goal and steps how to achieve that sub goal.
  - Use code samples and plenty of 📝, 🔎 and ❔ call-outs.
  - Use gender-neutral language and avoid words such as 'easy' or 'simple'.
- A homework markdown file named `{topic}-homework-{language}.md` in the `lessons/{language}/{topic}` folder. Example: _lessons/dotnet6/http/http-homework-dotnet6.md_.
- A new Functions project to the `src/{language}/{topic}/AzFuncUni.{Topic}` folder with a completely working and running example. E.g. _src/dotnet6/http/AzFuncUni.Http_. Try to have one project file for the entire lesson and use subfolders or classes for the different exercises.
- [CodeTour](https://marketplace.visualstudio.com/items?itemName=vsls-contrib.codetour) files (one per exercise) to the `tours./{language}/{topic}/.tours` folder.
- A [VSCode workspace](https://code.visualstudio.com/docs/editor/multi-root-workspaces#_workspace-file-schema) file in the `workspace/{language}` folder for the lesson. The workspace should contain these folders:
  - `lessons/{language}/prerequisites`
  - `lessons/{language}/{topic}`
  - `src/{language}/{topic}` (including REST client files, if applicable)
  - `.tours/{language}/{topic}`

We know, this is a loooong list! 😬 But you don't have to do everything by yourself! We can work together on this so please do reach out! 💪