import 'package:flutter/material.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';

class AppLabeledFormField<T> extends StatelessWidget {
  final String label;
  final T? initialValue;
  final FormFieldValidator<T>? validator;
  final Widget Function(FormFieldState<T> fieldState) builder;

  const AppLabeledFormField({
    super.key,
    required this.label,
    required this.builder,
    this.initialValue,
    this.validator,
  });

  @override
  Widget build(BuildContext context) {
    final inputTheme = Theme.of(context).inputDecorationTheme;
    final resolvedErrorStyle =
        inputTheme.errorStyle ??
        AppTextStyles.caption.copyWith(color: AppColors.error);

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text(label, style: AppTextStyles.label14InkSemiBold),
        SizedBox(height: AppDimensions.spacing8.h),
        FormField<T>(
          initialValue: initialValue,
          validator: validator,
          builder: (fieldState) {
            return Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                builder(fieldState),
                if (fieldState.hasError) ...[
                  SizedBox(height: 6.h),
                  Padding(
                    padding: EdgeInsetsDirectional.only(start: 12.w),
                    child: Text(
                      fieldState.errorText!,
                      style: resolvedErrorStyle,
                    ),
                  ),
                ],
              ],
            );
          },
        ),
      ],
    );
  }
}