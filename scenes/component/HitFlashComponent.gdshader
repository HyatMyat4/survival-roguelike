shader_type canvas_item;

uniform float flash_amount : hint_range(0.0, 1.0) = 0.0;

void fragment()
{
    vec4 texture_color = texture(TEXTURE, UV);

    vec4 red_color = vec4(
        1.0,
        0.0,
        0.0,
        texture_color.a
    );

    COLOR = mix(texture_color, red_color, flash_amount);
}