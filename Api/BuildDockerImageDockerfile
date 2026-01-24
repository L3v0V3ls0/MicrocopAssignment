# Use ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Create a non-root user for better security
ARG USERNAME=appuser
ARG USER_UID=1000
ARG USER_GID=$USER_UID

# Create the user
RUN groupadd --gid $USER_GID $USERNAME \
    && useradd --uid $USER_UID --gid $USER_GID -m $USERNAME \
    && apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

    
# Create directories for data and logs
RUN mkdir -p /logs 

# Switch to non-root user
USER $USERNAME

# Set working directory
WORKDIR /app

COPY publish/ ./
COPY data/ data/


# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_HTTP_PORTS=8080

# Expose port
EXPOSE 8080
    
# Define volume mounts (these will be created as anonymous volumes if not specified)
VOLUME ["/data", "/logs"]

# Entry point
ENTRYPOINT ["dotnet", "Api.dll"]