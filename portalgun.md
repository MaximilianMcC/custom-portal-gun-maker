# How to make a custom portal gun (manual)

## Download these:
1. [VTFEdit](https://developer.valvesoftware.com/wiki/VTFEdit) - used for converting vtf files
2. [GCFScape](https://developer.valvesoftware.com/wiki/GCFScape) - used for extracting game textures

## Extract the portal gun texture
Open GCFScape and open up either Portal 1, or Portal 2.
### Portal 1
1. Go to `./portal` and open `portal_pak_dir.vpk` in GCFScape.
2. Go to `./materials/models/weapons/v_models/v_portalgun/`.
3. Open `v_portalgun.vtf` with VTFEdit.
### Portal 2
1. Go to `./portal2` and open `pak01_dir.vpk` in GCFScape.
2. Go to `./materials/models/weapons/v_models/v_portalgun`.
3. Open `v_portalgun.vtf` with VTFEdit. You can also open `v_portalgun_blue.vtf` for ATLAS, and `v_portalgun_orange.vtf` for P-body

In VTFEdit, export the texture by going to `File -> Export` and saving it as `v_portalgun.png`.

## Edit the texture
Edit and draw on the texture however you want using whatever software you'd like. You can also get the normals and stuff from the same place where `v_portalgun.vtf` was taken from if you'd like to use those also.

## Convert back to vtf
Open your new image in VTFEdit, then go to `File -> Save As` and export it as `v_portalgun.vtf`.

## Add it back to the game
The steps here are very different for Portal 1 and Portal 2, so make sure you do the right one🤬🤬
### Portal 1
1. Go back to the `portal` directory.
2. make these directories: `custom/<PUT ANY NAME HERE>/materials/models/weapons/v_models/v_portalgun`.
3. Put the newly saved `v_portalgun.vtf` into the last directory.
### Portal 2
1. Make these directories: `pak01_dir/materials/models/weapons/v_models/v_portalgun`.
3. Put the newly saved `v_portalgun.vtf` into the last directory.
not done !!