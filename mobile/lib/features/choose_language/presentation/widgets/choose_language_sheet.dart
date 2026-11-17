import 'package:flutter/material.dart';
import 'package:flutter_animate/flutter_animate.dart';
import 'package:flutter_screenutil/flutter_screenutil.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/core/theme/app_dimensions.dart';
import 'package:healthcare_app/core/theme/app_text_styles.dart';
import 'package:healthcare_app/core/widgets/app_primary_button.dart';
import 'package:healthcare_app/generated/l10n.dart';

class ChooseLanguageSheet extends StatelessWidget {
  const ChooseLanguageSheet({
    super.key,
    required this.onSelectArabic,
    required this.onSelectEnglish,
  });

  final VoidCallback onSelectArabic;
  final VoidCallback onSelectEnglish;

  @override
  Widget build(BuildContext context) {
    final s = S.of(context);

    return Container(
      width: double.infinity,
      padding: EdgeInsetsDirectional.fromSTEB(
        AppDimensions.spacing24.w,
        AppDimensions.spacing16.h,
        AppDimensions.spacing24.w,
        32.h,
      ),
      decoration: BoxDecoration(
        color: AppColors.surface,
        borderRadius: BorderRadius.only(
          topLeft: Radius.circular(32.r),
          topRight: Radius.circular(32.r),
        ),
      ),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          Container(
            width: 38.w,
            height: 4.h,
            decoration: BoxDecoration(
              color: AppColors.line,
              borderRadius: BorderRadius.circular(2.r),
            ),
          ),
          SizedBox(height: AppDimensions.spacing24.h),
          Text(
            s.chooseLanguageTitleAr,
            style: AppTextStyles.title24InkBold,
            textAlign: TextAlign.center,
          ),
          SizedBox(height: AppDimensions.spacing6.h),
          Text(
            s.chooseLanguageTitleEn,
            style: AppTextStyles.body16MutedSemiBold,
            textAlign: TextAlign.center,
          ),
          SizedBox(height: AppDimensions.spacing24.h),
          AppPrimaryButton(
            text: s.languageArabicOption,
            backgroundColor: AppColors.languageOptionBg,
            textStyle: AppTextStyles.heading18InkSemiBold,
            borderSide: const BorderSide(color: AppColors.line),
            onPressed: onSelectArabic,
          ),
          SizedBox(height: AppDimensions.spacing12.h),
          AppPrimaryButton(
            text: s.languageEnglishOption,
            backgroundColor: AppColors.languageOptionBg,
            textStyle: AppTextStyles.heading18InkSemiBold,
            borderSide: const BorderSide(color: AppColors.line),
            onPressed: onSelectEnglish,
          ),
          SizedBox(height: AppDimensions.spacing24.h),
          Text(
            s.chooseLanguageHintAr,
            style: AppTextStyles.caption13MutedRegular,
            textAlign: TextAlign.center,
          ),
          SizedBox(height: AppDimensions.spacing4.h),
          Text(
            s.chooseLanguageHintEn,
            style: AppTextStyles.caption13MutedRegular,
            textAlign: TextAlign.center,
          ),
        ],
      ),
    ).animate().slideY(
      begin: 0.25,
      end: 0,
      duration: 450.ms,
      curve: Curves.easeOutCubic,
    );
  }
}
