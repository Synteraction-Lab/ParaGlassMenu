#!/bin/bash

# Set the FAN_REAL_TOKEN environment variable
export FAN_TOKEN="${{ secrets.FAN_REAL_TOKEN }}"

# Set the PLUG_REAL_TOKEN environment variable
export PLUG_TOKEN="${{ secrets.PLUG_REAL_TOKEN }}"

# Run your tests here
echo "Running tests with FAN_TOKEN=$FAN_TOKEN and PLUG_TOKEN=$PLUG_TOKEN"