# Kafka Microservices Project

This project demonstrates an event-driven microservices architecture using **Apache Kafka** for communication between services. The solution consists of three main services: **OrderService**, **PaymentService**, and **NotificationService**. Each service communicates asynchronously through Kafka topics, enabling loosely coupled interactions.

---

## Features

- **OrderService**: Handles order creation and publishes events to Kafka.
- **PaymentService**: Listens for order events, processes payments, and publishes payment status updates.
- **NotificationService**: Listens for payment status updates and logs appropriate notifications.
- **Kafka Integration**: Uses **Confluent.Kafka** client library for producing and consuming messages.
- **JSON Serialization**: Serializes and deserializes messages using `System.Text.Json`.
- **ASP.NET Core**: API and services implemented using ASP.NET Core Web API.

---

## Architecture Overview

- **OrderService**: 
  - Publishes events to the `OrderCreated` topic when an order is created.
- **PaymentService**:
  - Consumes events from the `OrderCreated` topic.
  - Processes payments based on business logic.
  - Publishes events to the `PaymentProcessed` topic.
- **NotificationService**:
  - Consumes events from the `PaymentProcessed` topic.
  - Logs notifications based on the payment outcome.

---

## Technologies Used

### Backend
- **ASP.NET Core 8**: Framework for building web APIs and services.
- **Confluent.Kafka**: Kafka client for producing and consuming messages.
- **System.Text.Json**: For lightweight JSON serialization and deserialization.
- **Task-based asynchronous programming**: Ensures non-blocking operations.

### Messaging
- **Apache Kafka**: Event streaming platform for messaging between microservices.

---

## Services

### 1. OrderService
- **Purpose**: Handles customer order creation.
- **Technology**: ASP.NET Core Web API.
- **Endpoint**:
  - `POST /api/order`: Creates an order and publishes a message to the `OrderCreated` topic.
- **Kafka Topic**: `OrderCreated`

### 2. PaymentService
- **Purpose**: Processes payments for created orders.
- **Technology**: Background service consuming Kafka messages.
- **Logic**: 
  - Approves payments below $10,000.
  - Rejects payments above $10,000.
- **Kafka Topics**:
  - **Consume**: `OrderCreated`
  - **Produce**: `PaymentProcessed`

### 3. NotificationService
- **Purpose**: Logs notifications based on payment outcomes.
- **Technology**: Background service consuming Kafka messages.
- **Kafka Topic**: `PaymentProcessed`

---

## Running the Services

1. **OrderService**:
   - Runs on `http://127.0.0.1:5005`.
   - Exposes an API for creating orders.
   - Produces messages to the `OrderCreated` Kafka topic.

2. **PaymentService**:
   - Runs on `http://127.0.0.1:5007`.
   - Consumes messages from the `OrderCreated` topic.
   - Produces messages to the `PaymentProcessed` topic.

3. **NotificationService**:
   - Runs as a background service.
   - Consumes messages from the `PaymentProcessed` topic.
   - Logs payment notifications.

---

## Prerequisites

1. **Kafka Installation**:
   - Install and run Apache Kafka locally or use a managed Kafka service like Confluent Cloud.
2. **.NET 8 SDK**:
   - Required to build and run the services.
3. **Configuration**:
   - Set up `appsettings.json` for each service with Kafka configuration:
     ```json
     {
         "Kafka": {
             "BootstrapServers": "localhost:9092",
             "OrderCreatedTopic": "OrderCreated",
             "PaymentProcessedTopic": "PaymentProcessed"
         }
     }
     ```

---
