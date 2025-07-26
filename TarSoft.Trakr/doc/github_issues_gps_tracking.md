# 📋 GitHub Issues for GPS Tracking Project

## EPIC 0: GPS Device Sourcing & Design
- [ ] Research commercial GPS tracker vendors (hidden, long-life IoT)
- [ ] Evaluate device protocols (MQTT/HTTP support, encryption)
- [ ] Assess battery performance and form factors
- [ ] Validate compatibility with ingestion backend
- [ ] Build PoC firmware for custom-designed device (optional)
- [ ] Shortlist 2–3 suppliers or partners for field testing
- [ ] Document provisioning spec: QR code, bootstrap token, cert model
- [ ] Ensure GDPR/CE/FCC compliance for consumer resale

## EPIC 1: Device Telemetry Pipeline
- [ ] Set up MQTT and HTTP ingestion gateways (ASP.NET Core 9)
- [ ] Integrate Kafka/Event Hub for ingestion stream
- [ ] Build telemetry processor service (.NET 9 Worker)
- [ ] Store data in TimescaleDB + PostGIS
- [ ] Real-time cache of “last known location” in Redis

## EPIC 2: Consent, Identity & Provisioning
- [ ] Configure Duende IdentityServer or Azure AD B2C
- [ ] Implement user/device registration and consent ledger
- [ ] Build provisioning API (claim QR, assign asset, validate token)
- [ ] Add RLS to DB + tenant middleware to API Gateway
- [ ] Log consent + revocation events

## EPIC 3: AI & Alert Engine
- [ ] Define anomaly rules (speed spikes, out-of-zone, no signal)
- [ ] Integrate ML model for maintenance prediction (ONNX or ML.NET)
- [ ] Build alert dispatcher + WebSocket push + email fallback
- [ ] User feedback flow for alert tuning

## EPIC 4: Frontends (Web & Mobile)
- [ ] Build web dashboard (Blazor or React) with live map
- [ ] Create timeline and trip view per device
- [ ] Support device registration and geofence config
- [ ] Build native MAUI or Flutter mobile app
- [ ] Connect WebSocket / SignalR for real-time updates

## EPIC 5: Analytics & Reporting
- [ ] Implement usage reporting API
- [ ] Generate asset movement heatmaps
- [ ] Allow CSV/PDF exports
- [ ] Integrate with Metabase or Power BI Embedded

## EPIC 6: Observability & DevOps
- [ ] Enable OpenTelemetry across services
- [ ] Centralize logs with Serilog + Seq
- [ ] Set up CI/CD pipelines for backend and frontend
- [ ] Containerize with .NET 9 Alpine images

