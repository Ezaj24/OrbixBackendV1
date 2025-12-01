📘 ORBIX Backend V1 – README

A lightweight, fast, AI-powered backend designed for the ORBIX application.
This backend provides:

🔹 Chat endpoint (general AI conversation)

🔹 Prompt refinement endpoint (rewriting in different styles & languages)

🔹 Free inference model using Groq Llama 3.1 8B Instant

🔹 System-prompt control

🔹 Environment-based configuration

🔹 Secure key isolation via environment variables

Built using .NET 9 Minimal Hosting Model and optimized for modern frontend AI interfaces.

🚀 Features
1. AI Chat Endpoint

POST /chat
Handles normal conversational queries.

2. Prompt Refinement Endpoint

POST /refine
Rewrites user input using:

ChatGPT style

Claude style

Coding

Creative

Marketing

Teaching

Languages supported:

English

Hindi

Urdu

Arabic

Spanish

3. Free, Fast AI Model

Uses the free-tier model:

llama-3.1-8b-instant

4. Secure

No keys stored in repo

Keys loaded from environment variables on server

appsettings.json contains no sensitive data

5. Deployment Ready

Designed for:

Render (free tier)

Railway

Azure App Service

Local hosting

🔧 Tech Stack
Layer	Technology
Backend	ASP.NET Core 9
AI Provider	Groq Llama 3.1 8B Instant
API Format	REST JSON
Hosting	Render
Language	C#
📂 Project Structure
/Controllers
   ChatController.cs
   RefineController.cs

/Services
   GroqService.cs
   PromptRefiner.cs

Program.cs
appsettings.json
README.md

🔌 API Endpoints
1. Chat
POST /chat
{
  "input": "Hello"
}


Response

{
  "response": "..."
}

2. Refine
POST /refine
{
  "input": "write job email",
  "mode": "chatgpt",
  "language": "english"
}


Response

{
  "refined": "..."
}

🛠️ Local Development
1. Restore
dotnet restore

2. Build
dotnet build

3. Run
dotnet run


Runs on:

http://localhost:5140

🔐 Environment Variables
GROQ_API_KEY=<your_key_here>


Add this in Render → Environment → Add Variable.

☁️ Deploying to Render

Create a new Web Service

Connect GitHub repo

Use these settings:

Setting	Value
Runtime	.NET
Build Command	dotnet build
Start Command	dotnet OrbixBackend.dll
Region	Singapore/Oregon
Branch	main

Add environment variable

Key: GROQ_API_KEY
Value: your_key_here


Deploy
Render provides a URL like:

https://orbix-backend.onrender.com

📌 Status

Backend: Complete

Free model in use

Fully compatible with ORBIX Frontend V1
