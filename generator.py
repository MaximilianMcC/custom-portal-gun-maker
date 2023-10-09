from PIL import Image


def generate_elevator(imagePath):
	
	# Elevator and image resize/position values
	WIDTH = 680
	HEIGHT = 186
	X = 236
	Y = 729

	# Open both the image, and the elevator
	image = Image.open(imagePath)
	elevator = Image.open("./img/templates/portal1/round_elevator_sheet_3.png")

	# Resize the image to fit on the elevator, then put it on the elevator
	image_resized = image.resize((WIDTH, HEIGHT))
	elevator.paste(image_resized, (X, Y))

	# Save the new image
	# TODO: Do in temp folder
	elevator.save("./test/round_elevator_sheet_3.png")
	elevator.show()

	# TODO: Convert to VTF

	# TODO: Put in mod texture pack correct file thing