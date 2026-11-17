import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';

class AppPrimaryButton extends StatelessWidget {
  final String text;
  final VoidCallback? onPressed;
  final IconData? suffixIcon;
  final IconData? prefixIcon;
  final bool isLoading;
  final Color? backgroundColor;
  final TextStyle? textStyle;
  final BorderSide? borderSide;

  const AppPrimaryButton({
    super.key,
    required this.text,
    required this.onPressed,
    this.suffixIcon,
    this.prefixIcon,
    this.isLoading = false,
    this.backgroundColor,
    this.textStyle,
    this.borderSide,
  });

  @override
  Widget build(BuildContext context) {
    final effectiveTextStyle = textStyle ?? AppTextStyles.body20WhiteSemiBold;
    final iconColor = effectiveTextStyle.color ?? Colors.white;

    return ElevatedButton(
      onPressed: isLoading ? null : onPressed,
      style: (backgroundColor != null || borderSide != null)
          ? ElevatedButton.styleFrom(
              backgroundColor: backgroundColor,
              side: borderSide,
            )
          : null,
      child: isLoading
          ? SizedBox(
              width: 22.w,
              height: 22.w,
              child: CircularProgressIndicator(
                strokeWidth: 2.5,
                color: iconColor,
              ),
            )
          : Row(
              mainAxisAlignment: MainAxisAlignment.center,
              mainAxisSize: MainAxisSize.min,
              children: [
                if (prefixIcon != null) ...[
                  Icon(prefixIcon, size: 18.sp, color: iconColor),
                  SizedBox(width: AppDimensions.spacing8.w),
                ],
                Text(text, style: effectiveTextStyle),
                if (suffixIcon != null) ...[
                  SizedBox(width: AppDimensions.spacing8.w),
                  Icon(suffixIcon, size: 18.sp, color: iconColor),
                ],
              ],
            ),
    );
  }
}