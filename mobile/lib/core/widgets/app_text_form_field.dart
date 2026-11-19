import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/helpers/extensions.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';

class AppTextFormField extends StatelessWidget {
  const AppTextFormField({
    super.key,
    required this.label,
    required this.hintText,
    this.controller,
    this.validator,
    this.prefixIcon,
    this.suffixIcon,
    this.obscureText = false,
    this.keyboardType,
    this.textInputAction,
    this.textDirection,
    this.helperText,
    this.hasError = false,
    this.onChanged,
  });

  final String label;
  final String hintText;
  final TextEditingController? controller;
  final String? Function(String?)? validator;
  final Widget? prefixIcon;
  final Widget? suffixIcon;
  final bool obscureText;
  final TextInputType? keyboardType;
  final TextInputAction? textInputAction;
  final TextDirection? textDirection;
  final String? helperText;
  final bool hasError;
  final ValueChanged<String>? onChanged;

  @override
  Widget build(BuildContext context) {
    final inputTheme = Theme.of(context).inputDecorationTheme;

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(label, style: AppTextStyles.label14InkSemiBold),
        SizedBox(height: AppDimensions.spacing8.h),
        TextFormField(
          controller: controller,
          validator: validator,
          obscureText: obscureText,
          keyboardType: keyboardType,
          textInputAction: textInputAction,
          textDirection: textDirection,
          onChanged: onChanged,
          style: AppTextStyles.body16InkRegular,
          decoration: InputDecoration(
            hintText: hintText,
            hintStyle: AppTextStyles.label14MutedRegular,
            helperText: ((!helperText.isNullOrEmpty())) ? helperText : null,
            helperStyle: AppTextStyles.caption12MutedRegular,
            prefixIcon: prefixIcon,
            suffixIcon: suffixIcon,
            enabledBorder: hasError ? inputTheme.errorBorder : null,
            focusedBorder: hasError ? inputTheme.focusedErrorBorder : null,
          ),
        ),
      ],
    );
  }
}
