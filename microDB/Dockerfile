# Use the official Microsoft SQL Server image
FROM mcr.microsoft.com/mssql/server:2019-latest

# Switch to root user to perform the necessary operations
USER root

# Copy your SQL script into the container
COPY init-db.sql /init-db.sql

# Copy the entrypoint script into the container
COPY entrypoint.sh /entrypoint.sh

# Make the entrypoint script executable
RUN chmod +x /entrypoint.sh

# Switch back to the mssql user
USER mssql

# Set the command to run your entrypoint script
CMD /bin/bash /entrypoint.sh
