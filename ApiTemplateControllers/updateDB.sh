#!/bin/bash

# Check if argument is provided
if [ $# -eq 0 ]; then
    echo "Usage: $0 <database_name> [environment]"
    exit 1
fi

# Get arguments
COMMIT_MSG=$1

# Basic argument validation
if [ -z "$COMMIT_MSG" ]; then
    echo "Error: Database name cannot be empty"
    exit 1
fi

echo "Migration message: $COMMIT_MSG"

dotnet ef migrations add $COMMIT_MSG

dotnet ef database update