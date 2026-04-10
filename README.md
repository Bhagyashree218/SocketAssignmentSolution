# Socket Assignment Solution

## Overview
This project is an async TCP client-server application built using .NET. It demonstrates socket communication with AES encryption, configuration management, and a structured service-based architecture.

## Features
- Async TCP server supporting multiple clients
- AES encryption for secure communication
- Configuration using appsettings.json
- File-based logging
- Clean service-based architecture
- Input validation and error handling

## Project Structure

### ServerApp
- SocketService – Handles TCP communication
- DataService – Business logic (SetA-Two mapping)
- EncryptionService – AES encryption/decryption
- LoggerService – File logging
- appsettings.json – Configuration

### ClientApp
- Sends encrypted request to server
- Receives and decrypts response

## How It Works

1. Client sends request in format:
   SetA-Two

2. Server:
   - Decrypts request
   - Extracts Set and Key
   - Fetches value from DataService
   - Sends current time response based on value

3. Client:
   - Decrypts response
   - Displays output

## Example

Input:
SetA-Two

Output:
13:40:01
13:40:02

## How to Run

### Server
- Run ServerApp

### Client
- Run ClientApp
- Enter input like:
  SetA-Two

## Technologies Used
- C#
- .NET
- TCP Sockets
- AES Encryption

## Author
Bhagyashree
