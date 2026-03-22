# Measurement Results Application - User Guide

## 1. Purpose

This application is used to store, manage, and evaluate measurement results of manufactured parts.  
Users can create, view, edit, and delete measurement records through a web interface.

---

## 2. Main Features

- Create new measurement results
- View measurement results with pagination
- Edit existing measurement data
- Delete measurement records
- Automatic status evaluation (PASS / NG)

---

## 3. Application Structure

The system consists of:

- Frontend: Angular web application
- Backend: ASP.NET REST API
- Database: MongoDB

All data is stored persistently in the database.

---

## 4. Navigation

### Home Page
Landing page of the application.

### Measurement Results Page
Main working page of the system.

Displays stored measurement results with detailed information.

---

## 5. Measurement Data Fields

Each measurement record contains:

- Part Name
- Serial Number
- Measurement Type
- Measured Value
- Unit
- Lower Limit
- Upper Limit
- Status (PASS / NG)
- Measured By
- Measured At (timestamp)
- Notes

---

## 6. Status Logic

The system automatically evaluates the measurement:

- PASS → value is within limits
- NG → value is outside limits

Users do not manually set the status.

---

## 7. Pagination

- The system displays one result per page
- Navigation buttons:

| Button | Function |
|------|--------|
| << | Jump to first page |
| Previous | Go to previous page |
| Next | Go to next page |
| >> | Jump to last page |

The number of pages depends on the total number of stored results.

---

## 8. Create New Measurement

1. Click: Creat New Measurement
2. Fill in all required fields
3. Click **Submit**

Result:
- The measurement is saved
- User is redirected to the first page
- The new record appears as the newest entry

---

## 9. Edit Measurement

1. Click **Edit** on a measurement result
2. Existing data is automatically loaded into the form
3. Modify the desired fields
4. Click **Save**

Result:
- Changes are saved
- User remains on the same page

To cancel editing:
- Click **Cancel**
- No changes are saved

---

## 10. Delete Measurement

1. Click **Delete**
2. Confirm the popup

Result:
- Record is permanently removed
- Page updates automatically

---

## 11. Data Persistence

- All data is stored in MongoDB
- Data remains available after application restart

---

## 12. Notes

- The system validates and processes all data through the backend API
- Frontend communicates with backend via HTTP requests
- Pagination and filtering are handled by the backend