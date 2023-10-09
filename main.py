# Imports
import sys
from generator import *

# TODO: GUI
# TODO: Don't use Python

# Get the image that they want to use
image = sys.argv[2]

# Get the thing that they want to make
thing = sys.argv[1]
if (thing == "elevator"):
	generate_elevator(image)