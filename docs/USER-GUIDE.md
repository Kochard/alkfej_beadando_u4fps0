# User Guide - Measurement Results System

## 1. Purpose

Measurements Results System provides an end-user experience for managing part measurement data.
Users can add, review, update, and delete measurement results in a simple UI.

## 2. Deployment assumptions

- Frontend is available at `http://localhost:8080`
- Backend API is available at `http://localhost:8080/api` (proxy setting in frontend)
- MongoDB is connected through the backend
- MCP service is available at `http://localhost:8081` (optional)

## 3. Core user workflow

### 3.1 View results

- Open the app in browser
- Navigate to "Measurement Results"
- Observe the table of records
- Use pagination buttons: First, Prev, Next, Last

### 3.2 Create result

1. Click "Create" or "New measurement".
2. Fill required fields: Part Name, Serial Number, Measurement Type, Measured Value, Lower/Upper Limit.
3. Optional: Unit, Measured By, Measured At, Notes.
4. Submit.
5. Confirm the new row appears in page 1.

### 3.3 Edit result

1. Click "Edit" on a row.
2. Update fields in the form.
3. Save changes.
4. Confirm table updates values.

### 3.4 Delete result

1. Click "Delete" on a row.
2. Confirm deletion.
3. Confirm row disappears.

## 4. Business logic

- Status is computed automatically:
  - PASS when `Lower Limit <= Measured Value <= Upper Limit`
  - NG otherwise
- This is shown as status in the list

## 5. Pagination UI

The list displays paged data with
- current page
- total pages
- total record count
- first/prev/next/last controls

## 6. MCP endpoints (optional)

If MCP is deployed and port-forwarded (default: http://localhost:8081):
- `GET /api/mcp/health` → service health status
- `GET /api/mcp/stats` → total/pass/fail counts and pass rate
- `GET /api/mcp/latest` → most recent measurements (default: last 5)
- `GET /api/mcp/insights` → measurement type distribution, top parts, recent trends
- `GET /api/mcp/anomalies` → measurements outside their limits, ranked by deviation
- `GET /api/mcp/predict/{partName}` → quality prediction for a specific part based on history

## 7. Troubleshooting

### A. List not loading

- Check network call in browser dev tools
- Ensure backend is running (`kubectl get pods -l app=backend`)
- Ensure Mongo is running (`kubectl get pods -l app=mongo`)

### B. Create/edit fails

- Check required fields are not empty
- Ensure measured value and limits are numeric
- Check backend API logs: `kubectl logs deployment/backend`

### C. MCP unreachable

- Check MCP pod: `kubectl get pods -l app=mcp`
- Ensure MCP service exists: `kubectl get svc mcp`

## 8. Quick validation commands

- `kubectl get pods`
- `kubectl get svc`
- `kubectl get deployments`
- `kubectl logs deployment/backend`
- `kubectl logs deployment/frontend`
- `kubectl logs deployment/mcp`

## 9. Support links

- Deployment reference: `docs/DEPLOYMENT-GUIDE.md`
- API tests: `requests/certificate-store.http`
- Bugs/features: GitHub issues


