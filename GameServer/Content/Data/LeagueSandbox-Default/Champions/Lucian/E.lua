Vector2 = require 'Vector2' -- include 2d vector lib 

function onFinishCasting()
    local current = Vector2:new(getOwnerX(), getOwnerY())
	local to = (Vector2:new(getSpellToX(), getSpellToY()) - current):normalize()
    local range = to * 425
    local trueCoords = current + range
	printChat("You used E");
 
    addParticle(getOwner(), "Lucian_E_Buf.troy", getOwnerX(), getOwnerY());
	
	-- ==============================================================================
	-- WARNING WARNING WARNING WARNING
	-- DASHTO HAS A VISUAL BUG WHICH TELEPORTS THE PLAYER TO THE MIDDLE OF THE MAP
	-- WHEN YOU CLICK TO MOVE THE VISUAL BUG DISSAPEARS
	-- ==============================================================================
	dashTo(getOwner(), trueCoords.x, trueCoords.y, 100);

	
end
 
function applyEffects()
    destroyProjectile()
end